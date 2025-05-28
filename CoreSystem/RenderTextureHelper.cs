using UnityEngine;

namespace NagaisoraFamework
{
	public static class RenderTextureHelper
	{
		public static byte[] SaveRenderTextureToPNG(RenderTexture Texture)
		{
			RenderTexture prev = RenderTexture.active;
			RenderTexture.active = Texture;
			Texture2D png = new Texture2D(Texture.width, Texture.height, TextureFormat.ARGB32, false);
			png.ReadPixels(new Rect(0, 0, Texture.width, Texture.height), 0, 0);

			byte[] bytes = png.EncodeToPNG();
			Object.DestroyImmediate(png);
			RenderTexture.active = prev;

			return bytes;
		}
	}
}
