using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
	public static Island instance;
	void Awake() { instance = this; }

	public bool randomSeed = false;
	public int seed;
	public int islandSize;
	public int carveAmt;
	public Vector2Int biomePerChunk;

	public List<Vector2Int> chunks = new List<Vector2Int>();
	public List<ChunkData> chunkDatas = new List<ChunkData>();
	public Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();
	public int tileCount;
	public List<Vector2Int> land = new List<Vector2Int>();
	public List<List<Vector2Int>> biomes = new List<List<Vector2Int>>();

	public void Start()
	{
		if(randomSeed)
			seed = Random.Range(int.MinValue, int.MaxValue);
		if (islandSize < 7)
			islandSize = 7;
		Random.InitState(seed);
		GenIslandData();
		Debug.Log(Time.realtimeSinceStartup);
	}
	public void GenIslandData()
	{
		AddChunks();
		AddTiles();
		Carve();
		Shrink();
		Clean();
		AddBiomes();
		DefineBiomes();
		//AddNoise();
		foreach (ChunkData cd in chunkDatas)
			cd.GetComponent<ChunkMesh>().GenMesh();
		tileCount = tiles.Count;
	}

	private void AddChunks()
	{
		List<Vector2Int> possLocs = new List<Vector2Int>();
		chunks.Add(Vector2Int.zero);
		chunks.AddRange(ChunkMath.FindAdjacentChunks(Vector2Int.zero));
		foreach(Vector2Int chunk in chunks)
		{
			foreach(Vector2Int adj in ChunkMath.FindAdjacentChunks(chunk))
			{
				if (!chunks.Contains(adj) && !possLocs.Contains(adj))
					possLocs.Add(adj);
			}
		}
		while (chunks.Count < islandSize)
		{
			Vector2Int toAdd = possLocs[Mathf.RoundToInt(Random.Range(0, possLocs.Count - 1))];
			int adjLand = 0;
			foreach(Vector2Int adj in ChunkMath.FindAdjacentChunks(toAdd))
			{
				if (chunks.Contains(adj))
					adjLand++;
			}
			if (adjLand >= 2)
			{
				chunks.Add(toAdd);
				possLocs.Remove(toAdd);
				foreach (Vector2Int v in ChunkMath.FindAdjacentChunks(toAdd))
				{
					if (!chunks.Contains(v) && !possLocs.Contains(v))
						possLocs.Add(v);
				}
			}
			else
			{
				possLocs.Remove(toAdd);
			}
		}
		foreach (Vector2Int chunk in chunks)
		{
			GameObject chunkGO = Instantiate(Resources.Load("ChunkPrefab")) as GameObject;
			ChunkData cd = chunkGO.GetComponent<ChunkData>();
			chunkDatas.Add(cd);
			cd.gridLoc = chunk;
			cd.transform.position = HexGrid.GridToWorld(cd.gridLoc);
			cd.transform.SetParent(transform);
			cd.Initialize();
		}
	}
	private void AddTiles()
	{
		foreach (Vector2Int chunk in chunks)
		{
			foreach (Vector2Int gl in HexGrid.FindWithinRadius(chunk, ChunkMath.chunkRadius))
			{
				tiles.Add(gl, 0);
				land.Add(gl);
			}
		}
	}
	private List<Vector2Int> GetEdgeTiles(List<Vector2Int> allTiles)
	{
		//find a coastal tile
		int index = allTiles.Count - 1;
		List<Vector2Int> toCheck = new List<Vector2Int>();
		List<Vector2Int> done = new List<Vector2Int>();
		List<Vector2Int> edgeTiles = new List<Vector2Int>();

		while (edgeTiles.Count == 0 && index > 0)
		{
			Vector2Int t = allTiles[index];
			foreach(Vector2Int adj in HexGrid.FindAdjacentGridLocs(t))
			{
				if (!allTiles.Contains(adj))
				{
					done.Add(t);
					edgeTiles.Add(t);
					toCheck.Add(t);
					break;
				}
			}
			index--;
		}

		while (toCheck.Count > 0)
		{
			Vector2Int t = toCheck[0];
			List<Vector2Int> adj = HexGrid.FindAdjacentGridLocs(t);
			bool c = false;
			foreach (Vector2Int a in adj)
			{
				if (!allTiles.Contains(a))
				{
					if(!edgeTiles.Contains(a))
						edgeTiles.Add(t);
					c = true;
					break;
				}
			}
			if (c)
			{
				foreach (Vector2Int a in adj)
				{
					if (allTiles.Contains(a) && !done.Contains(a) && !toCheck.Contains(a))
						toCheck.Add(a);
				}
			}

			toCheck.Remove(t);
			done.Add(t);
		}
		return edgeTiles;
	}
	private void Carve()
	{
		List<Vector2Int> edgeTiles = GetEdgeTiles(land);
		carveAmt = Mathf.RoundToInt(edgeTiles.Count * (6f / 10f));
		for (int i = 0; i < carveAmt; i++)
		{
			Vector2Int remove = edgeTiles[Random.Range(0, edgeTiles.Count - 1)];
			land.Remove(remove);
			edgeTiles.Remove(remove);
			tiles[remove] = -1;
			foreach (Vector2Int tile in HexGrid.FindAdjacentGridLocs(remove))
			{
				if (land.Contains(tile) && !edgeTiles.Contains(tile))
					edgeTiles.Add(tile);
			}
		}
	}
	private void Shrink()
	{
		List<Vector2Int> remove = GetEdgeTiles(new List<Vector2Int>(land));
		for (int i = 0; i < remove.Count; i++)
		{
			tiles[remove[i]] = -1;
			land.Remove(remove[i]);
		}
	}
	private void Clean()
	{
		List<Vector2Int> landTiles = new List<Vector2Int>();
		List<Vector2Int> toCheck = new List<Vector2Int>();
		List<Vector2Int> didCheck = new List<Vector2Int>();

		toCheck.Add(Vector2Int.zero);
		while(toCheck.Count > 0)
		{
			Vector2Int t = toCheck[0];
			if(land.Contains(t))
			{
				landTiles.Add(t);
				foreach(Vector2Int v in HexGrid.FindAdjacentGridLocs(t))
				{
					if(!landTiles.Contains(v) && !toCheck.Contains(v) && !didCheck.Contains(v))
					{
						toCheck.Add(v);
					}
				}
			}
			didCheck.Add(t);
			toCheck.Remove(t);
		}
		List<Vector2Int> temp = new List<Vector2Int>(land);
		foreach (Vector2Int v in temp)
		{
			if (!landTiles.Contains(v))
			{
				//Debug.Log("AHHA");
				tiles[v] = -1;
				land.Remove(v);
			}
		}
		//Remove holes and islands
		//remove any tile only connected to one other tile
		//yo
	}
	private void AddBiomes()
	{
		foreach(ChunkData chunk in chunkDatas)
		{
			int bc = Random.Range(biomePerChunk.x, biomePerChunk.y);
			List<Vector2Int> chunkTiles = new List<Vector2Int>(chunk.tiles);
			List<Vector2Int> water = new List<Vector2Int>();
			for (int i = 0; i < chunkTiles.Count; i++)
			{
				if (!land.Contains(chunkTiles[i]))
					water.Add(chunkTiles[i]);
			}
			foreach (Vector2Int w in water)
				chunkTiles.Remove(w);
			for (int i = 0; i < bc; i++)
			{
				Vector2Int t = chunkTiles[Random.Range(0, chunkTiles.Count)];
				biomes.Add(new List<Vector2Int>());
				biomes[biomes.Count - 1].Add(t);
				chunkTiles.Remove(t);
			}
		}
		List<Vector2Int> left = new List<Vector2Int>(land);
		for (int i = 0; i < biomes.Count; i++)
		{
			if (left.Contains(biomes[i][0]))
				left.Remove(biomes[i][0]);
		}
		List<List<Vector2Int>> possTiles = new List<List<Vector2Int>>();
		for (int i = 0; i < biomes.Count; i++)
		{
			possTiles.Add(new List<Vector2Int>());
			foreach(Vector2Int v in HexGrid.FindAdjacentGridLocs(biomes[i][0]))
			{
				if (left.Contains(v))
					possTiles[i].Add(v);
			}
		}
		while (left.Count > 0)
		{
			int bIndex = Random.Range(0, possTiles.Count);
			if (possTiles[bIndex].Count > 0)
			{
				int tIndex;
				if (possTiles[bIndex].Count == 1)
					tIndex = 0;
				else
					tIndex = Random.Range(0, possTiles[bIndex].Count / 2);

				Vector2Int add = possTiles[bIndex][tIndex];
				if (left.Contains(add))
				{
					biomes[bIndex].Add(add);
					left.Remove(add);
					foreach (Vector2Int v in HexGrid.FindAdjacentGridLocs(add))
					{
						if (left.Contains(v) && !possTiles[bIndex].Contains(v))
							possTiles[bIndex].Add(v);
					}
				}
				possTiles[bIndex].Remove(add);
			}
		}
	}
	private void DefineBiomes()
	{
		List<int> coastBiomes = new List<int>();
		List<Vector2Int> coastTiles = GetEdgeTiles(land);
		for (int i = 0; i < biomes.Count; i++)
		{
			foreach(Vector2Int t in GetEdgeTiles(biomes[i]))
			{
				if(coastTiles.Contains(t))
				{
					coastBiomes.Add(i);
					break;
				}
			}
		}
		for (int i = 0; i < biomes.Count; i++)
		{
			if(!coastBiomes.Contains(i))
			{
				int h = Random.Range(1, 4);
				foreach(Vector2Int v in biomes[i])
				{
					tiles[v] = 1;
				}
			}
		}
	}
	private void AddNoise()
	{
		foreach(Vector2Int loc in land)
		{
			tiles[loc] += Random.Range(0, 2);
		}
	}
	public int GetHeight(Vector2Int gridLoc)
	{
		if (tiles.ContainsKey(gridLoc))
			return tiles[gridLoc];
		else
			return -1;
	}
	public ChunkData FindChunk(Vector2Int gridLoc)
	{

		foreach(ChunkData cd in chunkDatas)
		{
			if(cd.tiles.Contains(gridLoc))
			{
				return cd;
			}
		}
		return null;
	}
}
