using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStateMachine : StateMachine
{
	private void Start()
	{
		ChangeState<DefaultControlState>();
	}
}
