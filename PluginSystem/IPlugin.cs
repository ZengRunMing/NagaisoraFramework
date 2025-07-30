using System;

namespace NagaisoraFramework.Plugins
{
	public interface IPlugin : IDisposable
	{
		PluginInfo PluginInfo { get; }
	}
}
