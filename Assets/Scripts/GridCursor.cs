using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCursor : MonoBehaviour
{
	public Vector2Int gridLoc;

	public static GridCursor instance;

	void Awake() { instance = this; }

	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Terrain")
		{
			transform.position = Grid.GridToWorld(Grid.RoundToGrid(hit.point), 0);
			gridLoc = Grid.RoundToGrid(transform.position);
		}
	}
}
