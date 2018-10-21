using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWallState : ControlState
{
	public bool buildingWall = false;
	public Vector3Int start;
	public Vector3Int end;
	public float angle;

	public BuildWallTool tool;

	public override void Start()
	{
		base.Start();
		tool = GridCursor.instance.GetComponent<BuildWallTool>();
	}

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
		if (Input.GetMouseButtonDown(1))
			ic.mousePos1 = Input.mousePosition;
		if (Input.GetMouseButton(1))
			ic.RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
		if (Input.GetMouseButtonUp(1) && Vector2.Distance(Input.mousePosition, ic.mousePos1) < 10)
		{
			if(!buildingWall)
				sm.ChangeState<DefaultControlState>();
			else
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
		tool.TurnOffWall();
		buildingWall = false;
		int count = Mathf.RoundToInt(Vector3Int.Distance(start, end));
		if (count == 0)
			Instantiate(Resources.Load("Wall"), start, Quaternion.identity);
		else
		{
			for (int i = 0; i <= count; i++)
			{
				Vector3Int loc = Grid.RoundToGrid(Vector3.Lerp(start, end, (float)i / (float)count));
				Instantiate(Resources.Load("Wall"), loc, Quaternion.identity);
			}
		}
	}

	void CancelWall()
	{
		tool.TurnOffWall();
		buildingWall = false;
	}

	void DrawWall()
	{
		if (end != GridCursor.instance.gridLoc)
		{
			end = GridCursor.instance.gridLoc;
			int x = Mathf.Abs(start.x - end.x);
			int z = Mathf.Abs(start.z - end.z);
			if (x < z)
				end.x = start.x;
			else
				end.z = start.z;
			tool.DrawWall(start, end);
		}
		Debug.DrawLine(start, end, Color.red);
	}
}