using System;
using UnityEngine;

namespace NagaisoraFramework.Miedia
{
	public class AudioControl : CommMonoScriptObject
	{
		public AudioSource AudioSource;
		public AudioClip AudioClip;

		public float Volume;

		public float SUB;

		public TimeSpan PlayStartTime;
		public TimeSpan LoopStartTime;
		public TimeSpan LoopEndTime;

		public float PlayStartSeconds => (float)PlayStartTime.TotalSeconds;
		public float LoopStartSeconds => (float)LoopStartTime.TotalSeconds;
		public float LoopEndSeconds => (float)LoopEndTime.TotalSeconds;

		public bool AudioExit;

		public void Awake()
		{
			MainSystem.AudioControl = this;
		}

		public void Start()
		{
			AudioSource = GetComponent<AudioSource>();
		}

		public void Update()
		{
			if (LoopEndTime.Ticks < LoopStartTime.Ticks)
			{
				AudioSource.loop = true;
			}
			else
			{
				AudioSource.loop = false;

				if (LoopEndTime.Ticks != 0)
				{
					if (AudioSource.time >= LoopEndSeconds)
					{
						AudioSource.time = LoopStartSeconds;
					}
				}
			}
		}

		public void FixedUpdate()
		{
			if (AudioExit)
			{
				if (AudioSource.volume > 0f)
				{
					AudioSource.volume -= SUB;
				}
				else
				{
					AudioSource.Stop();
					AudioExit = false;
				}
			}
			else
			{
				AudioSource.volume = Volume;
			}
		}

		public void SetAudioData(WaveBGMData Data)
		{
			AudioClip = Data.Data;
			PlayStartTime = Data.StartTime;
			LoopStartTime = Data.LoopStartTime;
			LoopEndTime = Data.LoopEndTime;
		}

		public void Play()
		{
			AudioSource.clip = AudioClip;
			AudioSource.time = PlayStartSeconds;
			AudioSource.Play();
		}

		public void Stop()
		{
			AudioSource.Stop();
		}

		public void Pause()
		{
			AudioSource.Pause();
		}

		public void Exit()
		{
			AudioExit = true;
		}
	}
}