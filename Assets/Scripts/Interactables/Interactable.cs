using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public Vector2Int gridLoc;
	public List<InteractionSpot> interactionSpots = new List<InteractionSpot>();
	public List<string> possibleActions = new List<string>();

	public virtual void Start()
	{
		foreach (InteractionSpot i in interactionSpots)
		{
			if (!InteractableController.instance.interactables.ContainsKey(gridLoc + i.gridLoc))
				InteractableController.instance.interactables.Add(gridLoc + i.gridLoc, new List<Interactable>());
			InteractableController.instance.interactables[gridLoc + i.gridLoc].Add(this);
		}
		Debug.Log(InteractableController.instance.interactables.Count);
	}

	public virtual void Interact(Unit unit, int index)
	{

	}
}
