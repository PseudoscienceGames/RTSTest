using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunkMesh))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class ChunkData : MonoBehaviour {
	public Vector2Int gridLoc;
	public int radius = 1;
	public int maxHeight;
	public Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();

	private void Start() {
		List<Vector2Int> t = HexGrid.FindWithinRadius(gridLoc, radius);
		for (int i = 0; i < t.Count; i++){
			Vector3 worldLoc = HexGrid.GridToWorld(t[i], 0);
			tiles.Add(t[i], Mathf.RoundToInt(Mathf.PerlinNoise((worldLoc.x + 500) / 10f, (worldLoc.z + 500) / 10f) * maxHeight));
		}
		GetComponent<ChunkMesh>().GenMesh();
	}
}
