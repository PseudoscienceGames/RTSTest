using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCursor : MonoBehaviour
{
	public Vector2Int gridLoc;
	public GameObject wallTool;
	public GameObject doorTool;

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

	public void SetWallTool(Vector3 start, float rot, float scale)
	{
		wallTool.SetActive(true);
		wallTool.transform.position = start;
		wallTool.transform.eulerAngles = new Vector3(0, rot, 0);
		wallTool.transform.localScale = new Vector3(scale, 1, 1);
	}

	public void TurnOffWall()
	{
		wallTool.SetActive(false);
	}

	public void RotateDoorTool()
	{
		doorTool.transform.Rotate(0, 30, 0);
	}
}
