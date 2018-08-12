using System.Collections;
using UnityEngine;

public abstract class State : MonoBehaviour
{
	public StateMachine sm;
	public InputController ic;

	private void Start()
	{
		ic = GetComponent<InputController>();
		sm = GetComponent<StateMachine>();
	}

	public virtual void Enter()
	{

	}
	public virtual void Exit()
	{

	}
}
