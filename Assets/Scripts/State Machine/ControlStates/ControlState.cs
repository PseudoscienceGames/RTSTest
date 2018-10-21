using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState : State
{
	public StateMachine sm;
	public InputController ic;

	public virtual void Start()
	{
		ic = GetComponent<InputController>();
		sm = GetComponent<StateMachine>();
	}
}
