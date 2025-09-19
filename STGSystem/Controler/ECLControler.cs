using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NagaisoraFramework.STGSystem
{
	public class ECLControler : IDisposable
	{
		public string ECLName;

		public STGControler STGControler;
		public STGComponment STGComponment;

		public AssemblySystem AssemblySystem;

		public List<IECLInterrupt> ECLInterrupts;
		public List<IECLCondition> ECLConditions;

		public Type ECLMain;
		public Type ECLInterface;

		public object ECLMainObject;
		public object ECLInterfaceObject;

		public MethodInfo OnStartMethod;
		public MethodInfo OnUpdateMethod;

		public bool IsRunning;

		public void Run()
		{
			OnStartMethod?.Invoke(ECLMainObject, null);

			IsRunning = true;
		}

		public void Stop()
		{
			IsRunning = false;
		}

		public ECLControler(Assembly assembly, STGControler controler, STGComponment componment = null)
		{
			STGControler = controler;
			STGComponment = componment;

			AssemblySystem = new AssemblySystem(assembly);
			ECLName = AssemblySystem.Name.FullName;
			Type[] types = AssemblySystem.ListAllExportedType();

			ECLInterrupts = new List<IECLInterrupt>();
			ECLConditions = new List<IECLCondition>();

			foreach (Type type in types)
			{
				if (type.Name == "Main")
				{
					ECLMain = type;

					OnStartMethod = ECLMain.GetMethod("OnStart");
					OnUpdateMethod = ECLMain.GetMethod("OnUpdate");
				}

				if (type.Name == "Interface")
				{
					ECLInterface = type;
				}
			}

			ECLInterfaceObject = AssemblySystem.CreateInstance(ECLInterface, new object[] { STGControler, this, STGComponment });
			ECLMainObject = AssemblySystem.CreateInstance(ECLMain, new object[] { ECLInterfaceObject });

			foreach (Type type in types)
			{
				if (type.IsClass && !type.IsAbstract && typeof(IECLInterrupt).IsAssignableFrom(type))
				{
					IECLInterrupt interrupt = (IECLInterrupt)AssemblySystem.CreateInstance(type, new object[] { ECLInterfaceObject });
					ECLInterrupts.Add(interrupt);
				}
				else if (type.IsClass && !type.IsAbstract && typeof(IECLCondition).IsAssignableFrom(type))
				{
					IECLCondition condition = (IECLCondition)AssemblySystem.CreateInstance(type, new object[] { ECLInterfaceObject });
					ECLConditions.Add(condition);
				}
			}
		}

		public ECLControler(ECLData ecldata, STGControler controler, STGComponment componment = null) : this(ecldata.ECLAssembly, controler, componment)
		{
			
		}

		public void OnInterrupt()
		{
			foreach (IECLInterrupt interrupt in ECLInterrupts)
			{
				if (!interrupt.Condition())
				{
					continue;
				}

				STGControler.Stop();
				interrupt.Execute();

				interrupt.Flag = true;
			}
		}

		public void OnCondition()
		{
			foreach (IECLCondition condition in ECLConditions)
			{
				if (!condition.LoopExecution && condition.Flag)
				{
					continue;
				}

				if (!condition.Condition())
				{
					continue;
				}

				condition.Execute();

				if (!condition.LoopExecution)
				{
					condition.Flag = true;
				}
			}
		}

		public void OnUpdate()
		{
			if (!IsRunning)
			{
				return;
			}

			OnInterrupt();

			OnUpdateMethod?.Invoke(ECLMainObject, null);

			OnCondition();
		}

		public void Dispose()
		{

		}

		public void SetInterfaceField<T>(string name, T value)
		{
			AssemblySystem.SetField(ECLInterface, ECLInterfaceObject, name, value);
		}

		public T GetInterfaceField<T>(string name)
		{
			return (T)AssemblySystem.GetField(ECLInterface, ECLInterfaceObject, name);
		}

		public void SetInterfaceProperty<T>(string name, T value)
		{
			AssemblySystem.SetProperty(ECLInterface, ECLInterfaceObject, name, value);
		}

		public T GetInterfaceProperty<T>(string name)
		{
			return (T)AssemblySystem.GetProperty(ECLInterface, ECLInterfaceObject, name);
		}
	}
}
