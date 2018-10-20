using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	public int stats;
	public Vector3 destination;

	public void SetDestination(Vector3 pos)
	{
		destination = pos;
		GetComponent<NavMeshAgent>().SetDestination(pos);
	}

	public void Interact(InteractionSpot spot)
	{
		SetDestination(Grid.GridToWorld(spot.gridLoc, 0));
		StartCoroutine(Move(spot));
	}

	IEnumerator Move(InteractionSpot spot)
	{
		while (Vector3.Distance(transform.position, destination) > 0.1f)
			yield return null;
		spot.interactable.Interact(this, 0);
	}
}