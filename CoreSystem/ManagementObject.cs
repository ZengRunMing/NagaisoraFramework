using System;
using System.Collections.Generic;

namespace NagaisoraFamework
{
	[Serializable]
	public class ManagementObject
	{
		public Dictionary<string, object> Properties;

		public ManagementObject()
		{
			Properties = new Dictionary<string, object>();
		}

		public void AddProperty(string name, object value)
		{
			if (Properties.ContainsKey(name))
			{
				return;
			}

			Properties.Add(name, value);
		}

		public void RemoveProperty(string name)
		{
			if (!Properties.ContainsKey(name))
			{
				return;
			}

			Properties.Remove(name);
		}

		public object GetValue(string name)
		{
			if (!Properties.ContainsKey(name))
			{
				return null;
			}

			return Properties[name];
		}

		public override string ToString()
		{
			string Str = "";

			foreach (KeyValuePair<string, object> ass in Properties)
			{
				object igsn = ass.Value ?? "Null";

				Str += ($"{ass.Key} : {PrintArray(igsn)}\n");
			}

			return Str;
		}

		static object PrintArray(object a)
		{
			if (a.GetType().BaseType != typeof(Array))
			{
				return a;
			}

			Array b = (Array)a;

			if (b.Length == 0)
			{
				return "Arrray NaN";
			}

			string str = "";
			for (int i = 0; i < b.Length; i++)
			{
				str += b.GetValue(i);

				if (i < (b.Length - 1))
				{
					str += ", ";
				}
			}

			return $"{a.GetType()} [{str}]";
		}
	}
}
