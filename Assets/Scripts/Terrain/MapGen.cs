using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
	public static MapGen instance;
	void Awake(){ instance = this; }
	public int mapSize;

	public int seed;
	public float scale;
	public int octaves;
	public float persistance;
	public float lacunarity;
	public Vector2 offset;

	public List<Vector2Int> chunkLocs = new List<Vector2Int>();
	
	public int GetHeight(Vector2Int gridLoc)
	{
		Vector3 worldLoc = HexGrid.GridToWorld(gridLoc);

		return Mathf.RoundToInt(Noise.FindHeight(worldLoc.x, worldLoc.z, seed, scale, octaves, persistance, lacunarity));
	}

	private void Start()
	{
		chunkLocs = ChunkMath.FindChunksWithinRadius(Vector2Int.zero, mapSize);
		
		foreach (Vector2Int chunkLoc in chunkLocs)
		{
			GameObject chunk = Instantiate(Resources.Load("Chunk")) as GameObject;
			chunk.transform.position = HexGrid.GridToWorld(chunkLoc, 0);
			chunk.transform.SetParent(transform);
			chunk.name = "Chunk " + chunkLoc;
		}
	}
}
