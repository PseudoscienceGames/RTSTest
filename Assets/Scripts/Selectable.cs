using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Selectable : MonoBehaviour
{
	public Vector2 screenPoint;
	public SelectionState sState;
	public SelectionMarker myMarker;

	private void Update()
	{
		screenPoint = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
	}

	void Start ()
	{
		GameObject.Find("Cam").GetComponent<InputController>().selectables.Add(this);
		myMarker = (Instantiate(Resources.Load("SelectionMarker")) as GameObject).GetComponent<SelectionMarker>();
		myMarker.transform.parent = GameObject.Find("Canvas").transform;
		myMarker.unit = transform;
	}

	public void ChangeState(SelectionState s)
	{
		if(s == SelectionState.Highlighted)
		{
			myMarker.Highlight();
			sState = SelectionState.Highlighted;
		}
		if (s == SelectionState.Selected)
		{
			myMarker.Select();
			sState = SelectionState.Selected;
		}
		if (s == SelectionState.NotSelected)
		{
			myMarker.Hide();
			sState = SelectionState.NotSelected;
		}
	}
}

public enum SelectionState { NotSelected, Selected, Highlighted };
