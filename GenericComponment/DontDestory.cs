using NagaisoraFramework;

public class DontDestory : CommMonoScriptObject
{
	public void Awake()
	{
		DontDestroyOnLoad(this);
	}
}
