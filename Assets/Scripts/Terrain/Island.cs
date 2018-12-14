using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
	public static Island instance;
	void Awake() { instance = this; }

	public int seed;
	public int islandSize;
	public int carveAmt;

	public List<Vector2Int> chunks = new List<Vector2Int>();
	public List<ChunkData> chunkDatas = new List<ChunkData>();
	public Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();
	public int tileCount;
	public List<Vector2Int> land = new List<Vector2Int>();

	public void Start()
	{
		seed = Random.Range(int.MinValue, int.MaxValue);
		if (islandSize < 7)
			islandSize = 7;
		Random.InitState(seed);
		GenIslandData();
	}
	public void GenIslandData()
	{
		AddChunks();
		AddTiles();
		Carve();
		Shrink();
		Tent();
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
	do this
	private void Clean()
	{
		//Remove holes and islands
		//remove any tile only connected to one other tile
		//yo
	}
	private void Tent()
	{
		List<Vector2Int> left = new List<Vector2Int>(land);
		int h = 0;
		while (left.Count > 0)
		{
			foreach(Vector2Int t in GetEdgeTiles(left))
			{
				tiles[t] = h;
				left.Remove(t);
			}
			h++;
		}
		foreach(Vector2Int t in land)
		{
			tiles[t] -= Random.Range(0, 2);
			if (tiles[t] < 0)
				tiles[t] = 0;
		}
	}
	private void AddBlob(Vector2Int gl, bool big)
	{
		tiles[gl] = 0;
		if(!land.Contains(gl))
			land.Add(gl);
		if (big)
		{
			foreach (Vector2Int adj in HexGrid.FindAdjacentGridLocs(gl))
			{
				tiles[adj] = 0;
				if(!land.Contains(adj))
					land.Add(adj);
			}
		}
	}
	public int GetHeight(Vector2Int gridLoc)
	{
		if (tiles.ContainsKey(gridLoc))
			return tiles[gridLoc];
		else
			return -1;
	}
}
