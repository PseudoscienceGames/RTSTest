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

			foreach (Vector3 vert in tile.verts)
				verts.Add(vert - transform.position);
			foreach (int tri in tile.tris)
				tris.Add(tri + triNum);
			triNum += 6;
			foreach (Vector2 uv in tile.uvs)
				uvs.Add(uv);
			if (!TerrainManager.instance.t.Contains(tile.tN))
				TerrainManager.instance.t.Add(tile.tN);
		}
		ExpandDoubles();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
		col.sharedMesh = mesh;
	}

	void AddTiles(){
		foreach (Vector2Int loc in cd.tiles)
			meshTiles.Add(loc, new MeshTile(this, loc));
	}

	void CalcTileMeshData(){
		foreach (MeshTile h in meshTiles.Values)
		{
			h.CalcVerts();
			h.CalcTopo();
		}
		foreach (MeshTile h in meshTiles.Values)
		{
			for (int i = 0; i < 6; i++)
			{
				DrawSide(h, i);
			}
		}
	}
	void DrawSide(MeshTile tile, int side)
	{
		if (meshTiles.ContainsKey(HexGrid.MoveTo(tile.gridLoc, side)))
		{
			MeshTile otherTile = meshTiles[HexGrid.MoveTo(tile.gridLoc, side)];
			Vector3 t1v1 = tile.verts[HexGrid.MoveDirFix(side - 1)];
			Vector3 t2v1 = otherTile.verts[HexGrid.MoveDirFix(side + 3)];
			Vector3 t1v2 = tile.verts[side];
			Vector3 t2v2 = otherTile.verts[HexGrid.MoveDirFix(side + 2)];

			if (t1v1.y > t2v1.y && t1v2.y > t2v2.y)
			{
				verts.Add(t1v1 - transform.position);
				verts.Add(t2v1 - transform.position);
				verts.Add(t1v2 - transform.position);
				verts.Add(t2v2 - transform.position);
				tris.Add(triNum);
				tris.Add(triNum + 1);
				tris.Add(triNum + 2);
				tris.Add(triNum + 2);
				tris.Add(triNum + 1);
				tris.Add(triNum + 3);
				triNum += 4;
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));

			}
			else if (t1v1.y > t2v1.y && t1v2.y <= t2v2.y)
			{
				verts.Add(t1v1 - transform.position);
				verts.Add(t2v1 - transform.position);
				verts.Add(t1v2 - transform.position);
				verts.Add(t2v2 - transform.position);
				tris.Add(triNum);
				tris.Add(triNum + 1);
				tris.Add(triNum + 2);
				tris.Add(triNum + 2);
				tris.Add(triNum + 1);
				tris.Add(triNum + 3);
				triNum += 4;
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
			}
			else if (t1v1.y <= t2v1.y && t1v2.y > t2v2.y)
			{
				verts.Add(t1v1 - transform.position);
				verts.Add(t2v1 - transform.position);
				verts.Add(t1v2 - transform.position);
				verts.Add(t2v2 - transform.position);
				tris.Add(triNum);
				tris.Add(triNum + 1);
				tris.Add(triNum + 2);
				tris.Add(triNum + 2);
				tris.Add(triNum + 1);
				tris.Add(triNum + 3);
				triNum += 4;
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
				uvs.Add(new Vector2(0.5f, 0.5f));
			}
		}
	}

	void ExpandDoubles()
	{
		List<Vector3> newVerts = new List<Vector3>();
		List<int> newTris = new List<int>();
		List<Vector2> newUVs = new List<Vector2>();
		foreach (int tri in tris)
		{
			newVerts.Add(verts[tri]);
			newTris.Add(newVerts.Count - 1);
			newUVs.Add(uvs[tri]);
		}
		uvs = newUVs;
		verts = newVerts;
		tris = newTris;
	}

}