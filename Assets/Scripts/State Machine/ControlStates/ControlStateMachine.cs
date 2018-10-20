using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStateMachine : StateMachine
{
	private void Start()
	{
		ChangeState<DefaultControlState>();
	}

	public void BuildWall()
	{
		ChangeState<BuildWallState>();
	}

	public void BuildDoor()
	{
		ChangeState<BuildDoorState>();
	}
}
