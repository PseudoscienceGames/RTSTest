using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState0 : State
{
	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
			sm.ChangeState<DefaultControlState>();
		else
			ic.DrawSelectionBox();
	}

	public override void Enter()
	{
		if (ic == null)
			ic = GetComponent<InputController>();
		if (!Input.GetKey(KeyCode.LeftShift))
			ic.selection.Clear();
		ic.mousePos1 = Input.mousePosition;
	}
	public override void Exit()
	{
		if (Vector2.Distance(ic.mousePos1, Input.mousePosition) < ic.boxStartDistance)
		{
			ic.RaycastSelect();
			Debug.Log(Vector2.Distance(ic.mousePos1, Input.mousePosition));
		}
		else
			ic.BoxSelect();
		ic.HideSelectionBox();
	}
}
