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

	public void GenMesh(){
		mesh = GetComponent<MeshFilter>().mesh;
		tiles = GetComponent<ChunkData>().tiles;
		foreach(Vector2Int t in tiles.Keys){
			DrawTile(t);
		}
		mesh.vertices = verts.ToArray();
		mesh.triangles = tris.ToArray();
		mesh.RecalculateNormals();
	}

	void DrawTile(Vector2Int tile){
		List<int> heights = new List<int>();
		for (int i = 0; i <= 5; i++){
			verts.Add(FindVertLoc(tile, HexGrid.MoveTo(tile, i), HexGrid.MoveTo(tile, i + 1)));
			uvs.Add(Vector2.zero);
			heights.Add(Mathf.RoundToInt(verts[verts.Count - 1].y));
		}

		if (heights[0] - heights[5] == heights[2] - heights[3])
			AddTris(1);
		else if (heights[1] - heights[0] == heights[3] - heights[4])
			AddTris(2);
		else
			AddTris(3);
		
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
			//List<int> hds = new List<int>(3);
			//if (tiles[hex] == tiles[otherHex1])
			//	hds.Add(0);
			//else if (Mathf.Abs(tiles[hex] - tiles[otherHex1]) == 1)
			//	hds.Add(1);
			//else
			//	hds.Add(2);
			//if (tiles[hex] == tiles[otherHex2])
			//	hds.Add(0);
			//else if (Mathf.Abs(tiles[hex] - tiles[otherHex2]) == 1)
			//	hds.Add(1);
			//else
			//	hds.Add(2);
			//if (tiles[otherHex1] == tiles[otherHex2])
			//	hds.Add(0);
			//else if (Mathf.Abs(tiles[otherHex1] - tiles[otherHex2]) == 1)
			//	hds.Add(1);
			//else
			//	hds.Add(2);
			////Debug.Log(hex + " " + hds[0] + " " + hds[1] + " " + hds[2]);
			//if ((hds[0] == 0 && hds[1] != 0) || hds[0] == 1 && hds[1] == 2 && hds[2] == 2)
			//	loc = Vector3.Lerp(loc, worldLoc3, 0.25f);
			//else if ((hds[1] == 0 && hds[2] != 0) || hds[1] == 1 && hds[0] == 2 && hds[2] == 2)
			//	loc = Vector3.Lerp(loc, worldLoc2, 0.25f);
			//else if ((hds[2] == 0 && hds[0] != 0) || hds[2] == 1 && hds[0] == 2 && hds[1] == 2)
			//	loc = Vector3.Lerp(loc, worldLoc1, 0.25f);
			loc.y = tiles[hex] * HexGrid.tileHeight;
			if ((tiles[otherHex1] == tiles[hex] - 1 && (tiles[otherHex2] != tiles[hex] + 1 && tiles[otherHex2] != tiles[otherHex1] - 1)) ||
				(tiles[otherHex2] == tiles[hex] - 1 && (tiles[otherHex1] != tiles[hex] + 1 && tiles[otherHex1] != tiles[otherHex2] - 1)))
				loc.y -= HexGrid.tileHeight;
		}
		return loc;
	}

	void AddTris(int i)
	{
		if (i == 1)
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
		else if (i == 2)
		{
			tris.Add(triNum + 1);
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum + 1);
			tris.Add(triNum + 3);
			tris.Add(triNum + 0);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			tris.Add(triNum + 0);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			tris.Add(triNum + 0);
			triNum += 6;
		}
		else if (i == 3)
		{
			tris.Add(triNum + 2);
			tris.Add(triNum + 3);
			tris.Add(triNum + 4);
			tris.Add(triNum + 2);
			tris.Add(triNum + 4);
			tris.Add(triNum + 1);
			tris.Add(triNum + 4);
			tris.Add(triNum + 5);
			tris.Add(triNum + 1);
			tris.Add(triNum + 5);
			tris.Add(triNum + 0);
			tris.Add(triNum + 1);
			triNum += 6;
		}
		else
			Debug.LogError("uhh...");
	}
}