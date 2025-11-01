using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFramework
{
	public class CoroutineManager : CommMonoScriptObject
	{
		public List<Coroutine> Coroutines;

		public Coroutine StartCoroutine<T>(IEnumerator<T> enumerator)
		{
			Coroutine coroutine = base.StartCoroutine(enumerator);

			Coroutines.Add(coroutine);
			return coroutine;
		}

		public new void StopCoroutine(Coroutine coroutine)
		{
			base.StopCoroutine(coroutine);
			Coroutines.Remove(coroutine);
		}
	}
}
