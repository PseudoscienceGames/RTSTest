using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour{
	Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();
	public List<Vector3> verts = new List<Vector3>();
	public List<int> tris = new List<int>();
	public List<Vector2> uvs = new List<Vector2>();
	public int triNum = 0;
	public Mesh mesh;
	public bool colapse;
	public bool expand;
	public bool deform;

	public void GenMesh(){
		mesh = GetComponent<MeshFilter>().mesh;
		tiles = GetComponent<ChunkData>().tiles;
		foreach(Vector2Int t in tiles.Keys){
			DrawTile(t);
		}
		if (colapse)
			CollapseDoubles();
		if (expand)
			ExpandDoubles();
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.RecalculateNormals();
	}

	void DrawTile(Vector2Int tile){
		List<int> heights = new List<int>();
		int tIndex = 0;
		for (int i = 0; i <= 5; i++) {
			verts.Add(FindVertLoc(tile, HexGrid.MoveTo(tile, i), HexGrid.MoveTo(tile, i + 1)));
			heights.Add(Mathf.RoundToInt(verts[verts.Count - 1].y / HexGrid.tileHeight));
			//Debug.Log(tiles[tile] + " " + heights[i] + " " + verts[verts.Count - 1].y);
			if (tiles[tile] == heights[i])
				tIndex += Mathf.RoundToInt(Mathf.Pow(2, i));
		}
		//Debug.Log(tile + " " + tiles[tile] + " " + tIndex);
		//uvs.Add(new Vector2(0.5f, 1f));
		//uvs.Add(new Vector2(1f, 1f));
		//uvs.Add(new Vector2(1f, 0f));
		//uvs.Add(new Vector2(0.5f, 0f));
		//uvs.Add(new Vector2(0f, 0f));
		//uvs.Add(new Vector2(0f, 1f));

		uvs.Add(new Vector2(0.5f, 1f));
		uvs.Add(new Vector2(0.75f, 0.75f));
		uvs.Add(new Vector2(0.75f, 0.25f));
		uvs.Add(new Vector2(0.5f, 0f));
		uvs.Add(new Vector2(0.25f, 0.25f));
		uvs.Add(new Vector2(0.25f, 0.75f));

		List<int> z1 = new List<int>() { 1, 3, 8, 9, 30, 33, 51, 54, 28, 35 };
		List<int> z2 = new List<int>() { 48, 4, 15, 12, 27, 32, 36,// 57 };//14 49
		List<int> z3 = new List<int>() { 2, 6, 16, 18, 24, 39, 45, 60, 14 };//7 56
		List<int> t1 = new List<int>() { 10, 13, 19, 21, 22, 23, 25, 29, 31, 34, 37, 40, 52, 53, 55, 61 };
		List<int> t2 = new List<int>() { 0, 5, 11, 17, 20, 26, 38, 41, 42, 43, 44, 46, 47, 50, 58, 59, 62, 63 };

		if (z1.Contains(tIndex))
			AddTris(1);
		else if (z2.Contains(tIndex))
			AddTris(2);
		else if (z3.Contains(tIndex))
			AddTris(0);
		else if (t1.Contains(tIndex))
			AddTris(4);
		else if (t2.Contains(tIndex))
			AddTris(3);
		else
			AddTris(5);

	}

	private Vector3 FindVertLoc(Vector2Int hex, Vector2Int otherHex1, Vector2Int otherHex2)
	{
		Vector3 worldLoc1 = HexGrid.GridToWorld(hex, 0);
		Vector3 worldLoc2 = HexGrid.GridToWorld(otherHex1, 0);
		Vector3 worldLoc3 = HexGrid.GridToWorld(otherHex2, 0);
		Vector3 loc = (worldLoc1 + worldLoc2 + worldLoc3) / 3f;
		loc.y = tiles[hex] * HexGrid.tileHeight;
		if (tiles.ContainsKey(otherHex1) && tiles.ContainsKey(otherHex2))
		{
			if (deform)
			{
				List<int> hds = new List<int>(3);
				if (tiles[hex] == tiles[otherHex1])
					hds.Add(0);
				else if (Mathf.Abs(tiles[hex] - tiles[otherHex1]) == 1)
					hds.Add(1);
				else
					hds.Add(2);
				if (tiles[hex] == tiles[otherHex2])
					hds.Add(0);
				else if (Mathf.Abs(tiles[hex] - tiles[otherHex2]) == 1)
					hds.Add(1);
				else
					hds.Add(2);
				if (tiles[otherHex1] == tiles[otherHex2])
					hds.Add(0);
				else if (Mathf.Abs(tiles[otherHex1] - tiles[otherHex2]) == 1)
					hds.Add(1);
				else
					hds.Add(2);
				//Debug.Log(hex + " " + hds[0] + " " + hds[1] + " " + hds[2]);
				if ((hds[0] == 0 && hds[1] != 0) || hds[0] == 1 && hds[1] == 2 && hds[2] == 2)
					loc = Vector3.Lerp(loc, worldLoc3, 0.25f);
				else if ((hds[1] == 0 && hds[2] != 0) || hds[1] == 1 && hds[0] == 2 && hds[2] == 2)
					loc = Vector3.Lerp(loc, worldLoc2, 0.25f);
				else if ((hds[2] == 0 && hds[0] != 0) || hds[2] == 1 && hds[0] == 2 && hds[1] == 2)
					loc = Vector3.Lerp(loc, worldLoc1, 0.25f);
			}
			loc.y = tiles[hex] * HexGrid.tileHeight;
			if ((tiles[otherHex1] == tiles[hex] - 1 && (tiles[otherHex2] != tiles[hex] + 1 && tiles[otherHex2] != tiles[otherHex1] - 1)) ||
				(tiles[otherHex2] == tiles[hex] - 1 && (tiles[otherHex1] != tiles[hex] + 1 && tiles[otherHex1] != tiles[otherHex2] - 1)))
				loc.y -= HexGrid.tileHeight;
		}
		return loc;
	}

	void AddTris(int i)
	{
		if (i == 0)
		{
			tris.Add(triNum);
			tris.Add(triNum + 1);
			tris.Add(triNum + 2);
			tris.Add(triNum);
			tris.Add(triNum + 2);
			tris.Add(triNum + 5);
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum + 5);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			triNum += 6;
		}
		else if (i == 1)
		{
			tris.Add(triNum);
			tris.Add(triNum + 1);
			tris.Add(triNum + 5);
			tris.Add(triNum + 1);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			tris.Add(triNum + 1);
			tris.Add(triNum + 2);
			tris.Add(triNum + 4);
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			triNum += 6;
		}
		else if (i == 2)
		{
			tris.Add(triNum);
			tris.Add(triNum + 1);
			tris.Add(triNum + 3);
			tris.Add(triNum + 1);
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			tris.Add(triNum);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			triNum += 6;
		}
		else if (i == 3)
		{
			tris.Add(triNum);
			tris.Add(triNum + 1);
			tris.Add(triNum + 5);
			tris.Add(triNum + 1);
			tris.Add(triNum + 3);
			tris.Add(triNum + 5);
			tris.Add(triNum + 1);
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			triNum += 6;
		}
		else if (i == 4)
		{
			tris.Add(triNum);
			tris.Add(triNum + 1);
			tris.Add(triNum + 2);
			tris.Add(triNum);
			tris.Add(triNum + 2);
			tris.Add(triNum + 4);
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			tris.Add(triNum);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			triNum += 6;
		}
		else if (i == 5)
		{
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
			tris.Add(0);
		
			triNum += 6;
		}
		else
			Debug.LogError("uhh...");
	}

	void CollapseDoubles()
	{
		List<Vector3> newVerts = new List<Vector3>();
		List<int> newTris = new List<int>();
		List<Vector2> newUVs = new List<Vector2>();
		foreach (int tri in tris)
		{
			bool add = true;
			foreach (Vector3 v in newVerts)
			{
				if (Vector3.Distance(v, verts[tri]) < 0.01f)
				{
					newTris.Add(newVerts.IndexOf(v));
					add = false;
				}
			}
			if (add)
			{
				newVerts.Add(verts[tri]);
				newTris.Add(newVerts.Count - 1);
			}
		}
		for (int i = 0; i < newVerts.Count; i++)
			newUVs.Add(Vector2.zero);
		uvs = newUVs;
		verts = newVerts;
		tris = newTris;

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