using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWallState : ControlState
{
	public bool buildingWall = false;
	public Vector2Int start;
	public Vector2Int end;
	public float angle;

	private void Update()
	{
		if (!ic.mouseOverUI && Input.GetMouseButtonDown(0))
			StartWall();
		if (Input.GetMouseButtonUp(0))
			EndWall();
		if (buildingWall)
			DrawWall();
		if (Input.GetKeyDown(KeyCode.Escape))
			sm.ChangeState<DefaultControlState>();
		if(Input.GetMouseButtonDown(1))
			ic.mousePos1 = Input.mousePosition;
		if (Input.GetMouseButton(1))
			ic.RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
		if (Input.GetMouseButtonUp(1) && Vector2.Distance(Input.mousePosition, ic.mousePos1) < 10)
		{
			sm.ChangeState<DefaultControlState>();
			CancelWall();
		}
	}

	void StartWall()
	{
		start = GridCursor.instance.gridLoc;
		buildingWall = true;
	}

	void EndWall()
	{
		buildingWall = false;
		GridCursor.instance.TurnOffWall();
		GameObject currentWall = Instantiate(Resources.Load("WallPrefab")) as GameObject;
		currentWall.transform.position = GridCursor.instance.wallTool.transform.position;
		currentWall.transform.rotation = GridCursor.instance.wallTool.transform.rotation;
		currentWall.transform.localScale = GridCursor.instance.wallTool.transform.localScale;
	}

	void CancelWall()
	{
		buildingWall = false;
		GridCursor.instance.TurnOffWall();
	}

	void DrawWall()
	{
		Vector3 sWorld = Grid.GridToWorld(start, 0);
		Vector3 eWorld = Grid.GridToWorld(GridCursor.instance.gridLoc, 0);
		angle = Mathf.Round(Vector3.SignedAngle(Vector3.right, eWorld - sWorld, Vector3.up));
		if (angle % 30 != 0)
		{
			Vector3 t = Vector3.right * (eWorld - sWorld).magnitude;
			angle = Mathf.Round(angle / 30f) * 30f;
			eWorld = (Quaternion.Euler(0, angle, 0) * t) + sWorld;
			eWorld = Grid.GridToWorld(Grid.RoundToGrid(eWorld), 0);
		}
		GridCursor.instance.SetWallTool(sWorld, angle, Vector3.Distance(sWorld, eWorld));
	}
}
