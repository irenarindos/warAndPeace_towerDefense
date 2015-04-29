using UnityEngine;
using System.Collections;

public class Utils 
{
	public static Texture2D makeCircle()
    {
		Texture2D text = new Texture2D(128,128, TextureFormat.RGBA32, false);
		for (int i = 0; i < 128; ++i)
		{
			for (int j = 0; j < 128; ++j)
			{
				float x = (i - 64)/64f;
				float y = (j - 64)/64f;
				if (Mathf.Abs(Mathf.Sqrt(x*x + y*y) - 1) < 0.01)
				{
					text.SetPixel(i, j, new Color(0,0,0,1-Mathf.Abs(Mathf.Sqrt(x*x + y*y) - 1)));
				}
				else
				{
					text.SetPixel(i, j, new Color(0, 0, 0, 0));
				}
			}
		}
		text.Apply();
	    return text;
    }
}
