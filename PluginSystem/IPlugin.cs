using System;

namespace NagaisoraFamework.Plugins
{
	public interface IPlugin : IDisposable
	{
		PluginInfo PluginInfo { get; }
	}
}
