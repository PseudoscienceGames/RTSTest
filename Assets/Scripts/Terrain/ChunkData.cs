using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunkMesh))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class ChunkData : MonoBehaviour {
	public Vector2Int gridLoc;
	public List<Vector2Int> tiles = new List<Vector2Int>();

	public void Initialize() {
		List<Vector2Int> t = HexGrid.FindWithinRadius(gridLoc, ChunkMath.chunkRadius);
		for (int i = 0; i < t.Count; i++)
			tiles.Add(t[i]);
	}
}
