using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionComponent : MonoBehaviour
{
	bool isSelecting = false;
	Vector2 mousePos1;

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			isSelecting = true;
			mousePos1 = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0))
		{
			isSelecting = false;
		}
	}

	public bool IsWithinSelectionBounds(GameObject gameObject)
	{
		if (!isSelecting)
			return false;
		Camera cam = Camera.main;
		Bounds viewportBounds = Utils.GetViewportBounds(cam, mousePos1, Input.mousePosition);

		return viewportBounds.Contains(cam.WorldToViewportPoint(gameObject.transform.position));
	}

	private void OnGUI()
	{
		if (isSelecting)
		{
			Rect rect = Utils.GetScreenRect(mousePos1, Input.mousePosition);
			Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
			Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
		}

	}
}
