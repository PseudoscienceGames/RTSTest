using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlState2 : ControlState
{
	private void Update()
	{
		if (Input.GetMouseButtonUp(2))
			sm.ChangeState<DefaultControlState>();
		else
			transform.position -= transform.TransformVector(new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")));
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
