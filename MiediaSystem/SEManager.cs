using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace NagaisoraFramework.Miedia
{
	public class SEManager : CommMonoScriptObject
	{
		public AudioMixerGroup Mixer;

		public float Volume;

		public SEAudio[] SEAudio;

		public IDictionary<string, SEAudio> ACD = new Dictionary<string, SEAudio>();
		public List<AudioSource> AS;

		public void Awake()
		{
			MainSystem.SEManager = this;

			Initialization();

			if (SEAudio == null)
			{
				Debug.LogWarning("参数 SEAudio 为空，将无法播放音效");
				return;
			}

			foreach (SEAudio audio in SEAudio)
			{
				if (audio.SEType == SEType.AsName)
				{
					ACD.Add(audio.Name, audio);
					continue;
				}

				ACD.Add(Enum.GetName(typeof(SEType), audio.SEType), audio);
			}
		}

		public void PlaySE(string name)
		{
			if (!ACD.ContainsKey(name)) // 检查音效名称是否存在于字典中
			{
				return;
			}

			AudioClip clip = ACD[name].AudioClip; // 获取音频剪辑

			foreach (AudioSource audio in AS)
			{
				if (!(audio.clip is null)) // 检查音频源的音频剪辑是否为 null
				{
					if (audio.clip != clip) // 如果当前音频源的音频剪辑不是要播放的剪辑
					{
						continue; // 跳过当前音频源
					}
				}

				if (audio.isPlaying) // 如果音频源正在播放
				{
					audio.Stop(); // 停止当前音频源的播放
				}

				audio.clip = clip; // 设置音频源的音频剪辑为要播放的剪辑
				MainSystem.SetAudioSourceScale(audio, 1f); // 设置音频源的时间缩放为 1（正常速度）
				if (ACD[name].TimeScale) // 如果音频剪辑需要时间缩放
				{
					MainSystem.SetAudioSourceScale(audio, Time.timeScale); // 设置音频源的时间缩放为当前时间缩放
				}

				audio.volume = Volume; // 设置音频源的音量为 SEManager 的音量
				audio.Play(); // 播放音频源
				return; // 成功播放音效后直接返回
			}

			AddAudioScource(AS.Count - 1); // 如果没有可用的音频源，则添加一个新的音频源
		}

		public void AddAudioScource(int a)
		{
			GameObject obj = new GameObject()
			{
				name = "SE-" + a.ToString("00")
			};

			AudioSource audio = obj.AddComponent<AudioSource>();

			audio.outputAudioMixerGroup = Mixer;

			AS.Add(audio);
		}

		public void Initialization()
		{
			AS = new List<AudioSource>();

			for (int a = 0; a < 16; a++)
			{
				AddAudioScource(a);
			}
		}

		public void SEReset()
		{
			for (int a = 0; a < 16; a++)
			{
				Destroy(AS[a].gameObject);
			}
			AS.Clear();
			Initialization();
		}
	}

	[Serializable]
	public struct SEAudio
	{
		public SEType SEType;
		public string Name;
		public bool TimeScale;
		public AudioClip AudioClip;

		public SEAudio(AudioClip audioClip, string name = "", bool timescale = false, SEType type = SEType.AsName)
		{
			if (type == SEType.AsName)
			{
				Name = name;
				SEType = SEType.AsName;
			}
			else
			{
				Name = Enum.GetName(typeof(SEType), type);
				SEType = type;
			}
			TimeScale = timescale;
			AudioClip = audioClip;
		}
	}
}