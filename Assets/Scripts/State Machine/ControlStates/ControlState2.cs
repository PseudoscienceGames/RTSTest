using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlState2 : State
{
	private void Update()
	{
		if (Input.GetMouseButtonUp(2))
			sm.ChangeState<DefaultControlState>();
		

	}
	public override void Enter()
	{
		
	}

	public override void Exit()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Terrain")
		{
			//ic.target.GetComponent<NavMeshAgent>().SetDestination(hit.point);
		}
	}
}
