using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState : State
{
	public StateMachine sm;
	public InputController ic;

	private void Start()
	{
		ic = GetComponent<InputController>();
		sm = GetComponent<StateMachine>();
	}
}
