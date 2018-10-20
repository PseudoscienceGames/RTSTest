using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableController : MonoBehaviour
{
	public static InteractableController instance;
	private void Awake(){instance = this;}
	public Dictionary<Vector2Int, List<Interactable>> interactables = new Dictionary<Vector2Int, List<Interactable>>();

	public void DisplayOptions(Vector2Int gridLoc)
	{
		foreach(Interactable i in interactables[gridLoc])
		{
			for (int s = 0; s < i.possibleActions.Count; s++)
			{
				AddButton(i, i.possibleActions[s]);
			}
		}
	}
	private void AddButton(Interactable i, string actionName)
	{
		GameObject currentButton = Instantiate(Resources.Load("ActionButton")) as GameObject;
		currentButton.transform.SetParent(transform);
		currentButton.GetComponentInChildren<Text>().text = actionName;
		currentButton.GetComponent<Button>().onClick.AddListener(delegate { InputController.instance.SetUnitInteraction(i.interactionSpots[0]); });
	}
}
