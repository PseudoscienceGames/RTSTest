using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Grid
{
	public static Vector3Int RoundToGrid(Vector3 worldLoc)
	{
		Vector3Int gridLoc = new Vector3Int();
		gridLoc.x = Mathf.RoundToInt(worldLoc.x);
		gridLoc.y = Mathf.RoundToInt(worldLoc.y);
		gridLoc.z = Mathf.RoundToInt(worldLoc.z);
		return gridLoc;
	}
}
