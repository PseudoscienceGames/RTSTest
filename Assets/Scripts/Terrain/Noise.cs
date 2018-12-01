using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {

public static float FindHeight(float x, float y, int sd, float sc, int o, float p, float l, Vector2 of)
	{
		if (sc <= 0)
			sc = 0.0001f;

		float amp = 1;
		float freq = 1;
		float noiseHeight = 0;

		for (int i = 0; i < o; i++)
		{
			float sampleX = x / sc * freq;
			float sampleY = y / sc * freq;

			float perlinValue = Mathf.PerlinNoise(sampleX + of.x, sampleY + of.y);
			noiseHeight += perlinValue * amp;

			amp *= p;
			freq *= l;
		}
		return noiseHeight;
	}
}
