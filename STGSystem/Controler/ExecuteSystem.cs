using System;
using System.Collections.Generic;

using System.Reflection;

namespace NagaisoraFramework.STGSystem
{
	public class ExecuteSystem : IDisposable
	{
		public string Name;

		public AssemblySystem AssemblySystem;

		public List<Condition> Conditions;

		public Type NFEMain;

		public object MainObject;

		public MethodInfo OnInitializing;
		public MethodInfo OnInitialized;

		public MethodInfo OnUpdateMethod;

		public ExecuteSystem(Assembly assembly, STGControler controler, STGComponent componment = null)
		{
			AssemblySystem = new AssemblySystem(assembly);
			Name = AssemblySystem.Name.FullName;
			Type[] types = AssemblySystem.ListAllExportedType();

			Conditions = new List<Condition>();

			foreach (Type type in types)
			{
				if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(NFEMain)))
				{
					NFEMain = type;
					OnInitializing = NFEMain.GetMethod("OnInitializing");
					OnInitialized = NFEMain.GetMethod("OnInitialized");
					OnUpdateMethod = NFEMain.GetMethod("OnUpdate");

					MainObject = AssemblySystem.CreateInstance(NFEMain, new object[] { this, controler, componment });
				}
			}
		}

		public void Initializeing()
		{
			OnInitializing?.Invoke(MainObject, null);
		}

		public void Initialized()
		{
			OnInitialized?.Invoke(MainObject, null);
		}

		public void OnCondition()
		{
			foreach (Condition condition in Conditions)
			{
				condition.ConditionExecute();
			}
		}

		public void OnUpdate()
		{
			OnUpdateMethod?.Invoke(MainObject, null);

			OnCondition();
		}

		public void Dispose()
		{

		}
	}
}
