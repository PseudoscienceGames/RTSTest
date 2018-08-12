using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	public virtual State CurrentState
	{
		get { return _currentState; }
		set { _currentState = value; }
	}

	protected State _currentState;
	protected bool _inTransition;

	public virtual T GetState<T> () where T : State
	{
		T target = GetComponent<T>();
		if (target == null)
		{
			target = gameObject.AddComponent<T>();
			target.enabled = false;
		}
		return target;
	}

	public virtual void ChangeState<T> () where T : State
	{
		if (CurrentState != null)
		{
			CurrentState.Exit();
			CurrentState.enabled = false;
		}
		CurrentState = GetState<T>();
		CurrentState.enabled = true;
		CurrentState.Enter();
		Debug.Log(GetState<T>().ToString());
	}

	protected virtual void Transition (State value)
	{
		if (_currentState == value || _inTransition)
			return;
	}

	private void Start()
	{
		ChangeState<DefaultControlState>();
	}
}
