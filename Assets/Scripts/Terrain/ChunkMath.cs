using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkMath
{
	public static int chunkRadius = 12;
	public static Vector2Int MoveChunk(Vector2Int gridLoc, int moveDir)
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

	public static Vector2Int MoveChunk(Vector2Int gridLoc, int moveDir, int distance)
	{
		for (int i = 0; i < distance; i++)
		{
			gridLoc = MoveChunk(gridLoc, moveDir);
		}
		return gridLoc;
	}

	public static int FindChunkDistance(Vector2Int fromLoc, Vector2Int toLoc)
	{//BROKE!!!================**=098))}{}{}{}{}
		int tempFromZ = (int)(0 - (fromLoc.x + fromLoc.y));
		int tempToZ = (int)(0 - (toLoc.x + toLoc.y));
		int distance = (int)(Mathf.Abs(fromLoc.x - toLoc.x) + Mathf.Abs(fromLoc.y - toLoc.y) + Mathf.Abs(tempFromZ - tempToZ)) / 2;
		return 0;// Mathf.RoundToInt((float)distance / (float)chunkRadius);
	}

	public static List<Vector2Int> FindChunksWithinRadius(Vector2Int gridLoc, int radius)
	{
		Vector3 worldLoc = HexGrid.GridToWorld(gridLoc);
		List<Vector2Int> locs = new List<Vector2Int>();
		List<Vector2Int> toTest = new List<Vector2Int>();
		toTest.Add(gridLoc);
		while(toTest.Count > 0)
		{
			Vector2Int testing = toTest[0];
			if(!locs.Contains(testing))
			{
				locs.Add(testing);
				foreach(Vector2Int l in FindAdjacentChunks(testing))
				{
					if (!toTest.Contains(l) && !locs.Contains(l) &&
						Vector3.Distance(worldLoc, HexGrid.GridToWorld(l)) <= radius)
						toTest.Add(l);
				}
			}
			toTest.RemoveAt(0);
		}

		return locs;
	}

	public static List<Vector2Int> FindAdjacentChunks(Vector2Int gridLoc)
	{
		List<Vector2Int> adjacentLocs = new List<Vector2Int>();
		for (int i = 0; i < 6; i++)
			adjacentLocs.Add(MoveChunk(gridLoc, i));
		return adjacentLocs;
	}
	public static Vector2Int FindNearestChunk(Vector2Int gridLoc)
	{
		int disFromOrign = HexGrid.FindFlatGridDistance(Vector2Int.zero, gridLoc);
		

		return gridLoc;//
	}
}
