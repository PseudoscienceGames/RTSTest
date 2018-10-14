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
		if (Input.GetMouseButtonDown(0))
			StartWall();
		if (Input.GetMouseButtonUp(0))
			EndWall();
		if (buildingWall)
			DrawWall();
		if (Input.GetKeyDown(KeyCode.Escape))
			sm.ChangeState<DefaultControlState>();
	}

	void StartWall()
	{
		start = GridCursor.instance.gridLoc;
		buildingWall = true;
	}

	void EndWall()
	{
		buildingWall = false;
	}

	void CancelWall()
	{
		buildingWall = false;
	}

	void DrawWall()
	{
		Vector3 sWorld = Grid.GridToWorld(start, 0);
		Vector3 eWorld = Grid.GridToWorld(GridCursor.instance.gridLoc, 0);
		angle = Mathf.Round(Vector3.SignedAngle(Vector3.right, eWorld - sWorld, Vector3.up));
		if (angle % 30 != 0)
		{
			Vector3 t = Vector3.right * (eWorld - sWorld).magnitude;
			Debug.Log(angle + " " + Mathf.Round(angle / 30f) * 30f);
			eWorld = (Quaternion.Euler(0, Mathf.Round(angle / 30f) * 30f, 0) * t) + sWorld;
		}
		Debug.DrawLine(sWorld, eWorld, Color.red);
		//else draw line to nearest 30
	}
}
