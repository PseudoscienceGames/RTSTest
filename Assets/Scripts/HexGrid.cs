using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HexGrid
{
	public static float hexRadius = 2f * 0.5773502691896258f;
	public static float sideRadius = 1f;
	public static float tileHeight = 2f/3f;
	public static float sqrt3 = Mathf.Sqrt(3);

	//Finds distance in number of hexes between the hex at grid location fromLoc and the hex at toLoc
	public static int FindFlatGridDistance(Vector2Int fromLoc, Vector2Int toLoc)
	{
		int tempFromZ = (int)(0 - (fromLoc.x + fromLoc.y));
		int tempToZ = (int)(0 - (toLoc.x + toLoc.y));
		int distance = (int)(Mathf.Abs(fromLoc.x - toLoc.x) + Mathf.Abs(fromLoc.y - toLoc.y) + Mathf.Abs(tempFromZ - tempToZ)) / 2;
		return distance;
	}

	//Takes grid location and converts it to world location
	public static Vector3 GridToWorld(Vector2Int gridLoc, int height)
	{
		int tempZ = (int)(0 - (gridLoc.x + gridLoc.y));
		Vector3 worldPos = new Vector3(0.5f * (gridLoc.y - tempZ) * sqrt3 * hexRadius, height * tileHeight, 1.5f * gridLoc.x * hexRadius);
		return worldPos;
	}
	public static Vector3 GridToWorld(Vector2Int gridLoc)
	{
		return GridToWorld(gridLoc, 0);
	}

		//Takes world location and converts it to grid location
		public static Vector2Int RoundToGrid(Vector3 worldLoc)
	{
		Vector2Int gridLoc = Vector2Int.zero;
		gridLoc.x = Mathf.RoundToInt(worldLoc.z / (1.5f * hexRadius));
		gridLoc.y = Mathf.RoundToInt((worldLoc.x / (sqrt3 * hexRadius)) - (gridLoc.x * 0.5f));// - (gridLoc.x * hexRadius * sqrt3 * 0.5f));
		return gridLoc;
	}

	//Fixes moveDir
	public static int MoveDirFix(int moveDir)
	{
		while (moveDir > 5)
			moveDir -= 6;
		while (moveDir < 0)
			moveDir += 6;
		return moveDir;
	}

	//Returns the grid location of the hex adjacent to the one at gridLoc in direction moveDir
	public static Vector2Int MoveTo(Vector2Int gridLoc, int moveDir)
	{
		moveDir = MoveDirFix(moveDir);
		Vector2Int moveTo = gridLoc;
		if (moveDir == 0)
		{
			moveTo.x++;
			moveTo.y--;
		}
		if (moveDir == 1)
			moveTo.x++;
		if (moveDir == 2)
			moveTo.y++;
		if (moveDir == 3)
		{
			moveTo.x--;
			moveTo.y++;
		}
		if (moveDir == 4)
			moveTo.x--;
		if (moveDir == 5)
			moveTo.y--;
		return moveTo;
	}

	public static Vector2Int MoveTo(Vector2Int gridLoc, int moveDir, int distance)
	{
		for (int i = 0; i < distance; i++)
		{
			gridLoc = MoveTo(gridLoc, moveDir);
		}
		return gridLoc;
	}

	public static List<Vector2Int> FindAdjacentGridLocs(Vector2Int gridLoc)
	{
		List<Vector2Int> adjacentLocs = new List<Vector2Int>();
		for (int i = 0; i < 6; i++)
			adjacentLocs.Add(MoveTo(gridLoc, i));
		return adjacentLocs;
	}

	public static List<Vector2Int> FindWithinRadius(Vector2Int gridLoc, int radius)
	{
		List<Vector2Int> locs = new List<Vector2Int>();
		locs.Add(gridLoc);
		for (int i = 1; i < radius; i++)
		{
			//Set initial hex grid location
			Vector2Int offset = new Vector2Int(i, -i);
			locs.Add(offset + gridLoc);
			int dir = 2;
			//Find data for each hex in the ring (each ring has 6 more hexes than the last)
			for (int fHex = 0; fHex < 6 * i; fHex++)
			{
				if (!locs.Contains(offset + gridLoc))
					locs.Add(offset + gridLoc);
				offset = MoveTo(offset, dir);
				if (offset.x == 0 || offset.y == 0 || offset.x == -offset.y)
				{
					dir++;
				}
			}
		}
		//Debug.Log(locs.Count);
		return locs;
	}

	public static List<Vector2Int> StraightPath(Vector2Int from, Vector2Int to)
	{

		List<Vector2Int> path = new List<Vector2Int>();
		Vector2Int at = new Vector2Int(from.x, from.y);
		while (at != to)
		{
			path.Add(at);
			Vector3 toWorld = GridToWorld(to);
			
			float distance = Vector3.Distance(GridToWorld(from), toWorld) + 1000;
			Vector2Int closest = Vector2Int.zero;
			for (int i = 0; i < 6; i++)
			{
				Vector2Int adjLoc = MoveTo(at, i);
				float dis = Vector3.Distance(GridToWorld(adjLoc), toWorld);
				if (dis < distance)
				{
					distance = dis;
					closest = adjLoc;
				}
			}
			at = closest;
		}
		return path;
	}
}