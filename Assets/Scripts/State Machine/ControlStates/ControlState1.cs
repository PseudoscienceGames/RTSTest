using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState1 : ControlState
{
	private void Update()
	{
		if (Input.GetMouseButtonUp(1))
			sm.ChangeState<DefaultControlState>();
		else
			ic.RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
	}

	public override void Enter()
	{
		if (ic == null)
			ic = GetComponent<InputController>();
		ic.mousePos1 = Input.mousePosition;
	}

	public override void Exit()
	{
		if (Vector2.Distance(Input.mousePosition, ic.mousePos1) < 10)
		{
			if (ic.selection.Count > 0)
			{
				ic.SetUnitDestinations();
			}
		}
	
	}
}
