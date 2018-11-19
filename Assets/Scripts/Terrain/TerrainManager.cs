using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {
	public static TerrainManager instance;
	void Awake() { instance = this; }

	public List<Vector2Int> st = new List<Vector2Int>();
	public Dictionary<Vector2Int, ChunkData> chunks = new Dictionary<Vector2Int, ChunkData>();
	public int chunkRadius = 16;
	public int maxHeight;
	public Transform loadTracker;
	public Vector2Int ltGridLoc;
	public Vector2Int centerChunk;
	public int minDis;
	public int dis;
	public GameObject chunkPrefab;
	public List<int> t = new List<int>();
	public bool topo;

	private void Start()
	{
		AddChunks(Vector2Int.zero);
		Invoke("S", 1);
	}

	void S()
	{
		t.Sort();
	}

	public int GetHeight(Vector2Int tile)
	{
		Vector3 worldLoc = HexGrid.GridToWorld(tile, 0);
		int h = Mathf.RoundToInt(Mathf.PerlinNoise((worldLoc.x + 500) / 10f, (worldLoc.z + 500) / 10f) * maxHeight);
		return h;
	}

	private void Update()
	{
		ltGridLoc = HexGrid.RoundToGrid(loadTracker.position);
		if (centerChunk != ltGridLoc)
		{
			minDis = HexGrid.FindFlatGridDistance(centerChunk, ltGridLoc);
			Vector2Int closestChunk = ltGridLoc;
			foreach (Vector2Int loc in chunks.Keys)
			{
				dis = HexGrid.FindFlatGridDistance(loc, ltGridLoc);
				//Debug.Log(dis + " " + minDis);
				if (dis < minDis)
				{
					Debug.Log("F");
					minDis = dis;
					closestChunk = loc;
				}
			}
			if(minDis < HexGrid.FindFlatGridDistance(closestChunk, centerChunk))
				MoveLoadTracker(closestChunk);

		}
		if(t.Count == 133)
		{
			string f = "";
			foreach (int i in t)
				f += i.ToString() + " ";
			Debug.Log(f);
		}
	}

	public void MoveLoadTracker(Vector2Int gridLoc)
	{
		centerChunk = gridLoc;
		DeleteChunks();
		AddChunks(gridLoc);
		Debug.Log(gridLoc);
	}

	Vector2Int MoveChunk(Vector2Int gridLoc, int moveDir)
	{
		Vector2Int moveTo = gridLoc;
		moveDir = HexGrid.MoveDirFix(moveDir);
		if (moveDir == 0)
		{
			moveTo.x += chunkRadius * 2 - 1;
			moveTo.y -= chunkRadius - 1;
		}
		if (moveDir == 1)
		{
			moveTo.x += chunkRadius - 1;
			moveTo.y += chunkRadius;
		}
		if (moveDir == 2)
		{
			moveTo.x -= chunkRadius;
			moveTo.y += chunkRadius * 2 - 1;
		}
		if (moveDir == 3)
		{
			moveTo.x -= chunkRadius * 2 - 1;
			moveTo.y += chunkRadius - 1;
		}
		if (moveDir == 4)
		{
			moveTo.x -= chunkRadius - 1;
			moveTo.y -= chunkRadius;
		}
		if (moveDir == 5)
		{
			moveTo.x += chunkRadius;
			moveTo.y -= chunkRadius * 2 - 1;
		}
		return moveTo;
	}

	void AddChunks(Vector2Int gridLoc)
	{
		st.Clear();
		List<Vector2Int> locs = new List<Vector2Int>();
		locs.Add(gridLoc);
		st.Add(gridLoc);
		for (int i = 0; i < 6; i++)
			locs.Add(MoveChunk(gridLoc, i));
		foreach (Vector2Int loc in locs)
		{
			GameObject chunk = Instantiate(chunkPrefab, HexGrid.GridToWorld(loc, 0), Quaternion.identity) as GameObject;
			chunks.Add(loc, chunk.GetComponent<ChunkData>());
		}
	}

	void DeleteChunks()
	{
		foreach (KeyValuePair<Vector2Int, ChunkData> kvp in chunks)
		{
			DestroyImmediate(kvp.Value.gameObject);
		}
		chunks.Clear();
		//Debug.Log(chunks.Count);
	}
}
