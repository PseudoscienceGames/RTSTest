using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
	public Vector2 mousePos1;
	public Vector2 mousePos2;
	public List<Selectable> selectables = new List<Selectable>();
	public List<Selectable> selection = new List<Selectable>();

	public RectTransform selectionBox;
	public bool selecting;

	public bool mouseOverUI;

	private void Update()
	{
		if (!mouseOverUI && Input.GetMouseButtonDown(0))
				StartSelectionBox();
		if (selecting)
		{
			if (Input.GetMouseButtonUp(0))
				EndSelectionBox();
			if (Input.GetMouseButton(0))
				DrawSelectionBox();
		}
	}

	void StartSelectionBox()
	{
		selecting = true;
		mousePos1 = Input.mousePosition;
	}
	void EndSelectionBox()
	{
		BoxSelect();
		selecting = false;
		selectionBox.position = new Vector2(-10, -10);
		selectionBox.sizeDelta = Vector2.one;
	}
	void DrawSelectionBox()
	{
		mousePos2 = Input.mousePosition;
		Vector2 mid = new Vector2((mousePos1.x + mousePos2.x) / 2f, (mousePos1.y + mousePos2.y) / 2f);
		Vector2 size = new Vector2(Mathf.Abs(mousePos1.x - mousePos2.x), Mathf.Abs(mousePos1.y - mousePos2.y));
		selectionBox.position = mid;
		selectionBox.sizeDelta = size;
	}

	void BoxSelect()
	{
		Bounds b = new Bounds(selectionBox.position, selectionBox.sizeDelta);
		foreach(Selectable s in selectables)
		{

			if (b.Contains(s.screenPoint))
				selection.Add(s);
		}
	}

	public void MouseOverUI()
	{
		mouseOverUI = true;
	}
	public void MouseNotOverUI()
	{
		mouseOverUI = false;
	}
}
