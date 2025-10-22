using UnityEngine;
using UnityEngine.UI;

using NagaisoraFramework;

public class RawImageUV : CommMonoScriptObject
{
    public float x;
    public float y;
    public float w;
    public float h;

    public RawImage RawImage;

	public void Start()
	{
        if (RawImage == null)
        {
            RawImage = GetComponent<RawImage>();
        }

        w = RawImage.uvRect.width;
        h = RawImage.uvRect.height;
	}

	public void FixedUpdate()
    {
        RawImage.uvRect = new Rect(RawImage.uvRect.x + x, RawImage.uvRect.y + y, w, h);
    }
}
