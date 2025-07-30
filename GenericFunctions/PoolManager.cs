using System;
using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFramework
{
	public class PoolManager
	{
		public Dictionary<Type, List<GameObject>> Stack = new Dictionary<Type, List<GameObject>>();

		public GameObject NewObject(Type Type)
		{
			GameObject Object;

			if (Stack.ContainsKey(Type))
			{
				if (Stack[Type].Count == 0)
				{
					return new GameObject();
				}
				Object = Stack[Type][0];
				Stack[Type].Remove(Object);
				return Object;
			}
			else
			{
				Stack.Add(Type, new List<GameObject>());
				return new GameObject();
			}
		}

		public void DeleteObject(Type Type, GameObject Object)
		{
			if (!Stack.ContainsKey(Type))
			{
				throw new NullReferenceException($"[{GetType()}] Type {Type} not found in PoolManager.");
			}
			
			Stack[Type].Add(Object);
			return;
		}

		public void ALLClear()
		{
			foreach (var item in Stack.Values)
			{
				foreach (var item1 in item)
				{
					GameObject.Destroy(item1);
				}
			}
			Stack.Clear();
		}
	}
}