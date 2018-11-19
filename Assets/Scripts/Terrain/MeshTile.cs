using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshTile
{
	public ChunkMesh cm;
	public Vector2Int gridLoc;
	public int h;
	public List<int> vh = new List<int>();
	public Vector3 worldLoc;
	public List<Vector3> verts = new List<Vector3>();
	public List<int> tris = new List<int>();
	public int tN = 0;
	public List<Vector2> uvs = new List<Vector2>();
	//public List<Vector3> normals = new List<Vector3>();

	public MeshTile(ChunkMesh c, Vector2Int g)
	{
		cm = c;
		gridLoc = g;
		h = TerrainManager.instance.GetHeight(gridLoc);
		worldLoc = HexGrid.GridToWorld(gridLoc, h);
	}

	public void CalcVerts()
	{
		for (int i = 0; i < 6; i++)
		{
			
			Vector2Int gridLoc2 = HexGrid.MoveTo(gridLoc, i);
			int h2;
			Vector3 worldLoc2;
			if (cm.meshTiles.ContainsKey(gridLoc2))
			{
				h2 = cm.meshTiles[gridLoc2].h;
				worldLoc2 = cm.meshTiles[gridLoc2].worldLoc;
			}
			else
			{
				h2 = TerrainManager.instance.GetHeight(gridLoc2);
				worldLoc2 = HexGrid.GridToWorld(gridLoc2, h2);
			}
			Vector2Int gridLoc3 = HexGrid.MoveTo(gridLoc, i + 1);
			int h3;
			Vector3 worldLoc3;
			if (cm.meshTiles.ContainsKey(gridLoc3))
			{
				h3 = cm.meshTiles[gridLoc3].h;
				worldLoc3 = cm.meshTiles[gridLoc3].worldLoc;
			}
			else
			{
				h3 = TerrainManager.instance.GetHeight(gridLoc3);
				worldLoc3 = HexGrid.GridToWorld(gridLoc3, h3);
			}
			
			Vector3 vert = (worldLoc + worldLoc2 + worldLoc3) / 3f;
			vert.y = h * HexGrid.tileHeight;
			if (TerrainManager.instance.topo)
			{
				if ((h2 == h - 1 && (h3 == h - 1 || h3 == h - 2)) ||
					(h2 == h && h3 == h - 1) ||
					((h3 == h - 1 && (h2 == h - 1 || h2 == h - 2)) ||
					(h3 == h && h2 == h - 1)) ||
					(h2 == h - 1 && h3 < h2) ||
					(h3 == h - 1 && h2 < h3) ||
					(h3 == h - 1 && h2 > h + 1) ||
					(h2 == h - 1 && h3 > h + 1))
				{ vert.y -= HexGrid.tileHeight; vh.Add(-1); }
				else if ((h2 == h + 1 && h3 == h + 2) ||
					(h2 == h + 2 && h3 == h + 1))
				{ vert.y += HexGrid.tileHeight; vh.Add(1); }
				else vh.Add(0);
			}
			verts.Add(vert);
		}
		uvs.Add(new Vector2(0.5f, 1f));
		uvs.Add(new Vector2(0.75f, 0.75f));
		uvs.Add(new Vector2(0.75f, 0.25f));
		uvs.Add(new Vector2(0.5f, 0f));
		uvs.Add(new Vector2(0.25f, 0.25f));
		uvs.Add(new Vector2(0.25f, 0.75f));
	}
	public void CalcTopo()
	{
		int levels = 0;
		List<List<int>> heights = new List<List<int>>();
		for (int i = 0; i < 3; i++)
		{
			heights.Add(new List<int>());
		}
		for (int i = 0; i < 6; i++)
		{
			if (vh[i] == vh.Max())
				heights[0].Add(i);
			else if (vh[i] == vh.Max() - 1)
				heights[1].Add(i);
			else
				heights[2].Add(i);
			levels += Mathf.RoundToInt((vh[i] - vh.Min()) * Mathf.Pow(3, i));
		}
		if (heights[0].Count == 6)
		{
			int[] t = { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 };
			tris.AddRange(t);
		}
		else if (heights[0].Count == 5)
		{
			int x = heights[1][0];
			int[] t = { 0, 1, 5, 5, 1, 2, 5, 2, 4, 2, 3, 4 };
			foreach (int y in t)
				tris.Add(HexGrid.MoveDirFix(y + x));
		}
		else if (heights[0].Count == 4)
		{
			if (heights[1][0] == HexGrid.MoveDirFix(heights[1][1] - 1))
			{
				int x = heights[1][0];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[1][0] == 0 && heights[1][1] == 5)
			{
				int x = heights[1][1];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[1][0] == HexGrid.MoveDirFix(heights[1][1] - 2))
			{
				int x = heights[1][0];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[1][0] == 0 && heights[1][1] == 4)
			{
				int x = heights[1][0];
				int[] t = { 0, 1, 5, 1, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[1][0] == 1 && heights[1][1] == 5)
			{
				int x = heights[0][3];
				int[] t = { 0, 1, 2, 2, 3, 4, 4, 5, 0, 0, 2, 4 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[1][0] == HexGrid.MoveDirFix(heights[1][1] - 3))
			{
				int x = heights[0][0];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else
			{
				int[] t = { 0, 1, 2, 2, 3, 4, 4, 5, 0 };
				tris.AddRange(t);
				Debug.Log("Not done 4");
			}
			//	//need more
			}
		here vvv
		else if (heights[0].Count == 3)
		{
			if (heights[0][0] == HexGrid.MoveDirFix(heights[0][1] - 1) &&
				heights[0][0] == HexGrid.MoveDirFix(heights[0][2] - 2))
			{
				int x = heights[0][0];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[0][0] == 0 && heights[0][1] == 1 && heights[0][2] == 5)
			{
				int x = heights[0][2];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[0][0] == 0 && heights[0][1] == 4 && heights[0][2] == 5)
			{
				int x = heights[0][1];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else
			{
				int[] t = { 0, 1, 2, 2, 3, 4, 4, 5, 0 };
				tris.AddRange(t);
				Debug.Log("Not done 3");
			}
			//need more
		}
		else if (heights[0].Count == 2)
		{
			if (heights[0][0] == HexGrid.MoveDirFix(heights[0][1] - 1))
			{
				int x = heights[0][0];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else if (heights[0][0] == 0 && heights[0][1] == 5)
			{
				int x = heights[0][1];
				int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else
			{
				int[] t = { 0, 1, 2, 2, 3, 4, 4, 5, 0 };
				tris.AddRange(t);
				Debug.Log("Not done 2");
			}
			//need more
		}
		else if (heights[0].Count == 1)
		{
			if (heights[1].Count == 5)
			{
				int x = heights[0][0];
				int[] t = { 0, 1, 5, 5, 1, 2, 5, 2, 4, 2, 3, 4 };
				foreach (int y in t)
					tris.Add(HexGrid.MoveDirFix(y + x));
			}
			else
			{
				int[] t = { 0, 1, 2, 2, 3, 4, 4, 5, 0 };
				tris.AddRange(t);
				Debug.Log("Not done1");
			}
			//need more
		}
		tN = levels;
	}
}