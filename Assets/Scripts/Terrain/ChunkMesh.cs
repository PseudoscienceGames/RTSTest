using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour{
	public Dictionary<Vector2Int, MeshTile> meshTiles = new Dictionary<Vector2Int, MeshTile>();
	public List<Vector3> verts = new List<Vector3>();
	public List<int> tris = new List<int>();
	public List<Vector2> uvs = new List<Vector2>();
	public int triNum = 0;
	public Mesh mesh;
	private ChunkData cd;
	private MeshCollider col;

	public void GenMesh(){
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();
		cd = GetComponent<ChunkData>();

		AddTiles();
		CalcTileMeshData();
		foreach(MeshTile tile in meshTiles.Values){
			verts.AddRange(tile.verts);
			foreach(int tri in tile.tris){
				tris.Add(tri + triNum);
			}
			triNum += 6;
		}
		ExpandDoubles();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		//mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		col.sharedMesh = mesh;
	}

	void AddTiles(){
		foreach (Vector2Int loc in cd.tiles)
			meshTiles.Add(loc, new MeshTile(this, loc));
	}

	void CalcTileMeshData(){
		foreach(MeshTile h in meshTiles.Values){
			h.CalcVerts();
			h.CalcTopo();
		}
	}

	void ExpandDoubles()
	{
		List<Vector3> newVerts = new List<Vector3>();
		List<int> newTris = new List<int>();
		//List<Vector2> newUVs = new List<Vector2>();
		foreach (int tri in tris)
		{
			newVerts.Add(verts[tri]);
			newTris.Add(newVerts.Count - 1);
			//newUVs.Add(uvs[tri]);
		}
		//uvs = newUVs;
		verts = newVerts;
		tris = newTris;
	}
}