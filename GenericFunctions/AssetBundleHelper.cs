using UnityEngine;

namespace NagaisoraFramework
{
	public class AssetBundleHelper
	{
		public AssetBundle LoadAssetBundleFromFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				Debug.LogError("File path is null or empty.");
				return null;
			}
			if (!System.IO.File.Exists(filePath))
			{
				Debug.LogError($"AssetBundle file not found at path: \"{filePath}\"");
				return null;
			}

			AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
			
			if (assetBundle == null)
			{
				Debug.LogError($"Failed to load AssetBundle from file: \"{filePath}\"");
			}
			
			return assetBundle;
		}

		public AssetBundle LoadAssetBundle(byte[] binary)
		{
			if (binary == null || binary.Length == 0)
			{
				Debug.LogError("Binary data is null or empty.");
				return null;
			}

			AssetBundle assetBundle = AssetBundle.LoadFromMemory(binary);

			if (assetBundle == null)
			{
				Debug.LogError("Failed to load AssetBundle from binary data.");
				return null;
			}

			return assetBundle;
		}

		public void UnloadAssetBundle(AssetBundle assetBundle, bool unloadAllLoadedObjects = false)
		{
			if (assetBundle == null)
			{
				Debug.LogWarning("AssetBundle is null, nothing to unload.");
				return;
			}
			assetBundle.Unload(unloadAllLoadedObjects);
			Debug.Log($"AssetBundle \"{assetBundle.name}\" unloaded successfully.");
		}

		public T LoadAsset<T>(AssetBundle assetBundle, string assetName) where T : Object
		{
			if (assetBundle == null)
			{
				Debug.LogError("AssetBundle is null.");
				return null;
			}
			
			T asset = assetBundle.LoadAsset<T>(assetName);

			if (asset == null)
			{
				Debug.LogError($"Asset \"{assetName}\" not found in AssetBundle \"{assetBundle.name}\".");
			}
			return asset;
		}
	}
}
