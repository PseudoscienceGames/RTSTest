using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieUnit : Unit
{
	public StateMachine sm;
	public State state;

	private void Start()
	{
		sm = GetComponent<StateMachine>();
		sm.ChangeState<IdleState>();
	}

}