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
	//public List<Vector2> uvs = new List<Vector2>();
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
			if ((h2 == h - 1 && (h3 == h - 1 || h3 == h - 2)) ||
				(h2 == h && h3 == h - 1) ||
				((h3 == h - 1 && (h2 == h - 1 || h2 == h - 2)) ||
				(h3 == h && h2 == h - 1)) ||
				(h2 == h - 1 && h3 < h2) ||
				(h3 == h - 1 && h2 < h3))
			{ vert.y -= HexGrid.tileHeight; vh.Add(-1); }
			else if ((h2 == h + 1 && h3 == h + 2) ||
				(h2 == h + 2 && h3 == h + 1))
			{ vert.y += HexGrid.tileHeight; vh.Add(1); }
			else vh.Add(0);
			verts.Add(vert);
		}
	}
	public void CalcTopo()
	{
		int levels = 0;
		for (int i = 0; i < 6; i++)
			levels += Mathf.RoundToInt((vh[i] + 1) * Mathf.Pow(3, i));
		Debug.Log(levels);

		//if(level[0] && level[3] )
		//{
			int[] t = { 0, 1, 2, 0, 2, 5, 2, 3, 5, 3, 4, 5 };
			for (int i = 0; i < t.Length; i++)
			{
				tris.Add(t[i]);
			}
			//DO STUFF HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//}
		//else if (level[1] && level[4])
		//{
		//	//DO STUFF HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//}
		//else if (level[2] && level[5])
		//{
		//	//DO STUFF HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//}
		//else if (level[0] && level[1])
		//{
		//	tris.Add(0);
		//	tris.Add(1);
		//	tris.Add(2);
		//	if(level[2])
		//	{
		//		tris.Add(0);
		//		tris.Add(2);
		//		tris.Add(3);
		//		if (level[5])
		//		{
		//			tris.Add(0);
		//			tris.Add(3);
		//			tris.Add(5);
		//			tris.Add(3);
		//			tris.Add(4);
		//			tris.Add(5);
		//		}
		//		else
		//		{
		//			tris.Add(0);
		//			tris.Add(3);
		//			tris.Add(4);
		//			tris.Add(0);
		//			tris.Add(4);
		//			tris.Add(5);
		//		}
		//	}
		//	else if(level[5])
		//	{
		//		tris.Add(0);
		//		tris.Add(2);
		//		tris.Add(5);
		//	}
		//	else if(level[3] && level[4])
		//	{
		//		tris.Add(0);
		//		tris.Add(2);
		//		tris.Add(3);
		//		tris.Add(0);
		//		tris.Add(3);
		//		tris.Add(5);
		//		tris.Add(3);
		//		tris.Add(4);
		//		tris.Add(5);
		//	}
		//}
		//if (true)//vh[0] == vh[1] && vh[1] == vh[2] && vh[3] != vh[0] && vh[5] != vh[1])
		//{
		//	tris.Add(0);
		//	tris.Add(1);
		//	tris.Add(2);
		//	tris.Add(0);
		//	tris.Add(2);
		//	tris.Add(5);
		//	tris.Add(2);
		//	tris.Add(3);
		//	tris.Add(5);
		//	tris.Add(3);
		//	tris.Add(4);
		//	tris.Add(5);
		//}
		//else
		//{
		//	for (int i = 0; i < 12; i++)
		//	{
		//		tris.Add(0);
		//	}
		//}
	}
}