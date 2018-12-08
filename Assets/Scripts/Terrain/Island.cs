using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
	public static Island instance;
	void Awake() { instance = this; }

	public int seed;
	public int islandSize;
	public int shrinkAmt;
	public int growAmt;

	public List<Vector2Int> chunks = new List<Vector2Int>();
	public List<ChunkData> chunkDatas = new List<ChunkData>();
	public Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();
	public int tileCount;
	public List<Vector2Int> land = new List<Vector2Int>();
	public List<Vector2Int> coastTiles = new List<Vector2Int>();

	public void Start()
	{
		seed = Random.Range(int.MinValue, int.MaxValue);
		if (islandSize < 7)
			islandSize = 7;
		Random.InitState(seed);
		StartCoroutine(AddChunks());
	}

	public IEnumerator GenIslandData()
	{
		AddTiles();
		coastTiles = GetCoastTiles();
		//Shrink(100);
		//Shrink(20);
		//Shrink(20);
		Shrink2();
		//AddNoise();
		foreach (ChunkData cd in chunkDatas)
		{
			cd.GetComponent<ChunkMesh>().GenMesh();
			yield return null;
		}
		tileCount = tiles.Count;
		yield return null;
	}

	private IEnumerator AddChunks()
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
		yield return null;
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
		yield return null;
		foreach (Vector2Int chunk in chunks)
		{
			GameObject chunkGO = Instantiate(Resources.Load("ChunkPrefab")) as GameObject;
			ChunkData cd = chunkGO.GetComponent<ChunkData>();
			chunkDatas.Add(cd);
			cd.gridLoc = chunk;
			cd.transform.position = HexGrid.GridToWorld(cd.gridLoc);
			cd.transform.SetParent(transform);
			cd.Initialize();
			yield return null;
		}
		StartCoroutine(GenIslandData());
		yield return null;
	}
	private void AddTiles()
	{
		foreach (Vector2Int chunk in chunks)
		{
			foreach (Vector2Int gl in HexGrid.FindWithinRadius(chunk, ChunkMath.chunkRadius))
			{
				if (tiles.ContainsKey(gl))
				{
					Debug.Log("wtf?" + gl);
				}
				else
				{
					tiles.Add(gl, 0);
					land.Add(gl);
				}
			}
		}
	}
	//private void Shrink(int amt)
	//{
	//	List<Vector2Int> possTiles = new List<Vector2Int>();
	//	List<Vector2Int> coastTiles = new List<Vector2Int>();
	//	int l = land.Count - 1;
	//	while(coastTiles.Count == 0)
	//	{
	//		Vector2Int loc = land[l];
	//		for (int i = 0; i < 6; i++)
	//		{
	//			if(!land.Contains(HexGrid.MoveTo(loc, i)))
	//			{
	//				coastTiles.Add(loc);
	//				break;
	//			}
	//		}
	//	}
	//	Debug.Log(coastTiles[0]);
	//	//foreach (Vector2Int gridLoc in land)
	//	//{
	//	//	bool edge = false;
	//	//	for (int i = 0; i < 6; i++)
	//	//	{
	//	//		if (!land.Contains(HexGrid.MoveTo(gridLoc, i)))
	//	//		{
	//	//			edge = true;
	//	//			break;
	//	//		}
	//	//	}
	//	//	if (edge)
	//	//		possTiles.Add(gridLoc);
	//	//}
	//	//List<int> toRemove = new List<int>();
	//	//for (int i = 0; i < possTiles.Count; i++)
	//	//	toRemove.Add(i);
	//	//for (int i = 0; i < possTiles.Count * ((100f - amt) / 100f); i++)
	//	//	toRemove.RemoveAt(Random.Range(0, toRemove.Count - 1));
	//	//foreach (int gl in toRemove)
	//	//{
	//	//	tiles[possTiles[gl]] = -1;
	//	//	if (land.Contains(possTiles[gl]))
	//	//		land.Remove(possTiles[gl]);
	//	//}
	//}

	private List<Vector2Int> GetCoastTiles()
	{
		List<Vector2Int> tiles = new List<Vector2Int>();
		//figure out what goes here
		return tiles;
	}
	private void Shrink2()
	{
		List<Vector2Int> coastTiles = new List<Vector2Int>();
		List<Vector2Int> possTiles = new List<Vector2Int>();
		int l = land.Count - 1;
		while (coastTiles.Count == 0)
		{
			Vector2Int loc = land[l];
			for (int i = 0; i < 6; i++)
			{
				if (!land.Contains(HexGrid.MoveTo(loc, i)))
				{
					coastTiles.Add(loc);
					break;
				}
			}
			l--;
		}
		possTiles.AddRange(HexGrid.FindAdjacentGridLocs(coastTiles[0]));
		int x = 0;
		while (possTiles.Count > 0 && x < 10000)
		{
			x++;
			Vector2Int loc = possTiles[0];
			List<Vector2Int> adjLocs = HexGrid.FindAdjacentGridLocs(loc);
			bool coast = false;
			if (land.Contains(loc))
			{
				foreach (Vector2Int adj in adjLocs)
				{
					if (!land.Contains(adj))
						coast = true;
				}
			}
			else
				possTiles.Remove(loc);
			if (coast)
			{
				coastTiles.Add(loc);
				possTiles.Remove(loc);
				foreach (Vector2Int adj in adjLocs)
				{
					if (land.Contains(adj))
						possTiles.Add(adj);
				}
			}
			else
				possTiles.Remove(loc);
		}
		for (int i = 0; i < shrinkAmt * islandSize; i++)
		{
			Vector2Int remove = coastTiles[Random.Range(0, coastTiles.Count - 1)];
			land.Remove(remove);
			coastTiles.Remove(remove);
			tiles[remove] = -1;
			foreach (Vector2Int tile in HexGrid.FindAdjacentGridLocs(remove))
			{
				if (land.Contains(tile) && !coastTiles.Contains(tile))
					coastTiles.Add(tile);
			}
		}
	}
	private void Clean()
	{
		//Remove holes and islands
		//remove any tile only connected to one other tile
		//yo
	}
	private void AddNoise()
	{
		foreach (Vector2Int v in land)
			tiles[v] = 3;// Random.Range(0, 3);
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
