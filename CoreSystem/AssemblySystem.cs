using System;
using System.Reflection;

namespace NagaisoraFramework
{
	public class AssemblySystem
	{
		public Assembly Assembly;

		public AssemblyName Name => Assembly.GetName();

		public AssemblySystem(Assembly assembly)
		{
			Assembly = assembly;
		}

		public AssemblySystem(byte[] binarys) : this(Assembly.Load(binarys))
		{

		}

		public object EntryPointRun(object obj, params object[] args)
		{
			return Assembly.EntryPoint.Invoke(obj, args);
		}

		public Type GetAssemblyType(string name)
		{
			return Assembly.GetType(name);
		}

		public MethodInfo GetMethod(Type type, string name)
		{
			return type.GetMethod(name);
		}

		public T RunMethod<T>(Type type, string name, object obj, params object[] args)
		{
			return (T)GetMethod(type,name)?.Invoke(obj, args);
		}

		public T RunMethod<T>(string typename, string name, object obj, params object[] args)
		{
			Type type = GetAssemblyType(typename);
			return RunMethod<T>(type, name, obj, args);
		}

		public object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}

		public object CreateInstance(string name)
		{
			Type type = GetAssemblyType(name);
			return CreateInstance(type);
		}
	}
}
