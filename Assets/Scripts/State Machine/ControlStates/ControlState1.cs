using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState1 : State
{
	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
			sm.ChangeState<DefaultControlState>();
		ic.RotateCamera(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
	}
}
