namespace NagaisoraFramework
{
	public static class CommonFunctions
	{
		public static string GetSzieString(ulong Size)
		{
			string s = string.Empty;

			if (Size < 1024ul)
			{
				s = $"{Size}B";
			}
			else if (Size < 1024ul * 1024ul)
			{
				s = $"{(Size / 1024f):0.00}KiB";
			}
			else if (Size < 1024ul * 1024ul * 1024ul)
			{
				s = $"{(Size / 1024f / 1024f):0.00}MiB";
			}
			else if (Size < 1024ul * 1024ul * 1024ul * 1024ul)
			{
				s = $"{(Size / 1024f / 1024f / 1024f):0.00}GiB";
			}
			else
			{
				s = $"{(Size / 1024f / 1024f / 1024f / 1024f):0.00}TiB";
			}

			return s;
		}
	}
}
