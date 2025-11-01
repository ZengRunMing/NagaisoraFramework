using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class ReplaySystem : IDisposable
	{
		public STGControler STGControler;

		public uint GameTime;

		public ReplayActionData[] ActionDatas;

		public bool IsRecording = false;
		public bool IsReplaying = false;

		public Vector2 LastVector = Vector2.zero;
		public Dictionary<uint, ReplayActionData> Actions = null;

		public ushort LastKeys;
		public ushort DownKeys;

		public InputKey InputKey;

		public ReplaySystem(STGControler controler)
		{
			STGControler = controler;

			STGControler.KeyDown += KeyDown;
		}

		public void Dispose()
		{
			STGControler.KeyDown -= KeyDown;
		}

		public void OnUpdate()
		{
			if (IsReplaying)
			{
				if (Actions.ContainsKey(GameTime))
				{
					ReplayActionData data = Actions[GameTime];
					DownKeys = data.DownKeys;

					InputKey = new InputKey();
					InputKey.FromHex(DownKeys);
				}

				STGControler.CallKeyDown(InputKey);
			}
		}

        public void KeyDown(InputKey inputKey)
        {
			if (IsRecording)
			{
				if (Actions.ContainsKey(GameTime))
				{
					return;
				}

				ushort nowkeys = inputKey.ToHex();

				if (nowkeys == LastKeys)
				{
					return;
				}

				Actions.Add(GameTime, new ReplayActionData(GameTime, nowkeys));
				LastKeys = nowkeys;
			}
		}

		public void RecordStart()
        {
            Actions = new Dictionary<uint, ReplayActionData>();
			IsRecording = true;
        }

		public void RecordContinue()
		{
			IsRecording = true;
		}

		public void RecordStop()
		{
            ActionDatas = Actions.Values.ToArray();
            IsRecording = false;
		}

        public void ReplayStart()
        {
			if (ActionDatas == null)
			{
				return;
			}

			Actions = new Dictionary<uint, ReplayActionData>();

			foreach(ReplayActionData actionData in ActionDatas)
			{
				Actions.Add(actionData.GameTime, actionData);
			}

			IsReplaying = true;
        }

		public void ReplayStop()
		{
			IsReplaying = false;
		}

		public void ReplayContinue()
		{
			IsReplaying = true;
		}
	}
}