using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	Texture2D map;
	public int dim = 1024;
	public float scale;
	public List<Octave> octaves = new List<Octave>();
	public int seed;
	public int maxHeight;

	private void Start()
	{
		Random.InitState(seed);
		map = new Texture2D(dim, dim);
		map.filterMode = FilterMode.Point;
		map.wrapMode = TextureWrapMode.Clamp;
		GenMap();
		map.Apply();
		Renderer r = GetComponent<Renderer>();
		r.sharedMaterial.mainTexture = map;
	}

	void GenMap()
	{
		for (int x = 0; x < dim; x++)
		{
			for (int y = 0; y < dim; y++)
			{
				float h = 0;
				foreach (Octave o in octaves)
				{
					float sX = x / o.scale * o.frequency + o.offset.x;
					float sY = y / o.scale * o.frequency + o.offset.y;
					h += Mathf.PerlinNoise(sX, sY);
				}
				h /= octaves.Count;
				map.SetPixel(x, y, new Color(0, Mathf.Round(h * maxHeight) / maxHeight, 0));
			}
		}
		
	}
	[System.Serializable]
	public class Octave
	{
		public Vector2 offset;
		public float scale;
		public float amplitude;
		public float frequency;
	}
}
