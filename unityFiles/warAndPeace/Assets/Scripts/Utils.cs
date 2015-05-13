using UnityEngine;
using System.Collections.Generic;

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
				if (Mathf.Abs(Mathf.Sqrt(x*x + y*y) - 1) < 0.005)
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

	public static void midpointDisplacement(Vector2 a, Vector2 b, IList<Vector2> output, float displacement, float threshold)
	{
		if (displacement < threshold)
		{
			output.Add(b);
			return;
		}
		float r = (Random.value-0.5f)*displacement;
		Vector2 midpoint = (a + b)/2;
		Vector2 dir = (a-b).normalized;
		Vector2 dispdir = new Vector2(dir.x, -dir.y);
		midpoint += dispdir*r;
		midpointDisplacement(a, midpoint, output, displacement/2, threshold);
		midpointDisplacement(midpoint, b, output, displacement/2, threshold);
	}
}
