using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCursor : MonoBehaviour
{
	public Vector2Int gridLoc;
	public bool up;

	public static GridCursor instance;

	void Awake() { instance = this; }

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			up = !up;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Terrain")
		{
			Vector2Int loc = HexGrid.RoundToGrid(hit.point);
			transform.position = HexGrid.GridToWorld(loc, Island.instance.GetHeight(loc));
			gridLoc = HexGrid.RoundToGrid(transform.position);
			if (Input.GetMouseButtonDown(0))
			{
				ChunkData cd = hit.transform.GetComponent<ChunkData>();
				if (up)
					cd.ChangeHeight(gridLoc, Island.instance.tiles[gridLoc] + 1);
				else
					cd.ChangeHeight(gridLoc, Island.instance.tiles[gridLoc] - 1);
				foreach(Vector2Int adj in HexGrid.FindAdjacentGridLocs(gridLoc))
				{
					if(!cd.tiles.Contains(adj))
					{
						Island.instance.FindChunk(adj).GetComponent<ChunkMesh>().GenMesh();
					}
				}

			}
		}
	}
}