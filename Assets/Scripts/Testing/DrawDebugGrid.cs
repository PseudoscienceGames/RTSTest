using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDebugGrid : MonoBehaviour
{
	public int radius;
	List<Vector3> points = new List<Vector3>();

	void Start ()
	{
		points.Add(Grid.GridToWorld(new Vector2Int(0, 0), 0));
		for (int i = 1; i < radius; i++)
		{
			//Set initial hex grid location
			Vector2Int gridLoc = new Vector2Int(i, -i);
			points.Add(Grid.GridToWorld(gridLoc, 0));
			int dir = 2;
			//Find data for each hex in the ring (each ring has 6 more hexes than the last)
			for (int fHex = 0; fHex < 6 * i; fHex++)
			{
				if (!points.Contains(Grid.GridToWorld(gridLoc, 0)))
					points.Add(Grid.GridToWorld(gridLoc, 0));
				gridLoc = Grid.MoveTo(gridLoc, dir);
				if (gridLoc.x == 0 || gridLoc.y == 0 || gridLoc.x == -gridLoc.y)
				{
					dir++;
				}
			}
		}

		foreach(Vector3 point in points)
		{
			Debug.DrawLine(point, point + Vector3.up, Color.red, 1000);
		}
	}

}
