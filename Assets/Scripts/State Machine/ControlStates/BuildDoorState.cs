using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDoorState : ControlState
{

	private void Update()
	{
		GridCursor.instance.doorTool.transform.position = GridCursor.instance.transform.position;
		if (!ic.mouseOverUI && Input.GetMouseButtonDown(0))
			PlaceDoor();
		if (Input.GetKeyDown(KeyCode.R))
			GridCursor.instance.RotateDoorTool();
		if (Input.GetKeyDown(KeyCode.Escape))
			sm.ChangeState<DefaultControlState>();
		if (Input.GetMouseButtonDown(1))
			ic.mousePos1 = Input.mousePosition;
		if (Input.GetMouseButton(1))
			ic.RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
		if (Input.GetMouseButtonUp(1) && Vector2.Distance(Input.mousePosition, ic.mousePos1) < 10)
		{
			sm.ChangeState<DefaultControlState>();
		}
	}

	void PlaceDoor()
	{
		GameObject currentDoor = Instantiate(Resources.Load("DoorPrefab")) as GameObject;
		currentDoor.transform.position = GridCursor.instance.doorTool.transform.position;
		currentDoor.transform.rotation = GridCursor.instance.doorTool.transform.rotation;
		currentDoor.transform.localScale = GridCursor.instance.doorTool.transform.localScale;
	}

	public override void Enter()
	{
		base.Enter();
		GridCursor.instance.doorTool.SetActive(true);
	}

	public override void Exit()
	{
		base.Exit();
		GridCursor.instance.doorTool.SetActive(false);
	}


}
