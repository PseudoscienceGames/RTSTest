using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSpot : MonoBehaviour
{
	public Vector2Int gridLoc;
	public Vector2Int facing;
	public Interactable interactable;

	private void Start()
	{
		interactable = transform.parent.GetComponent<Interactable>();
	}
}
