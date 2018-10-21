using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public static InputController instance;
	private void Awake(){instance = this;}

	public State state;

	public Vector2 mousePos1;
	public Vector2 mousePos2;
	public int boxStartDistance;
	public List<Selectable> selectables = new List<Selectable>();
	public List<Selectable> selection = new List<Selectable>();

	public RectTransform selectionBox;

	public bool mouseOverUI;

	public float zoom;
	public int zoomMin;
	public int zoomMax;
	public float zoomSpeed;
	public float cameraRotSpeed;
	public float camPanSpeed;
	public int scrollAreaSize;
	public bool screenEdgeScroll;

	public GameObject cameraPivot;

	private void Start()
	{
		MoveCamera(transform.position);
	}
	private void Update()
	{
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
		{
			MoveCamera(transform.position + transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))));
		}
	}
	public void MoveCamera(Vector3 loc)
	{
		Ray ray = new Ray(loc + (Vector3.up * 100), Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Terrain")
		{
			transform.position = hit.point;
		}
	}

	public void DrawSelectionBox()
	{
		mousePos2 = Input.mousePosition;
		Vector2 mid = new Vector2((mousePos1.x + mousePos2.x) / 2f, (mousePos1.y + mousePos2.y) / 2f);
		Vector2 size = new Vector2(Mathf.Abs(mousePos1.x - mousePos2.x), Mathf.Abs(mousePos1.y - mousePos2.y));
		SetSelectionBox(mid, size);
		Bounds b = new Bounds(selectionBox.position, selectionBox.sizeDelta);
		foreach (Selectable s in selectables)
		{

			if (b.Contains(s.screenPoint))
				s.ChangeState(SelectionState.Highlighted);
			else
			{
				if (selection.Contains(s))
					s.ChangeState(SelectionState.Selected);
				else
					s.ChangeState(SelectionState.NotSelected);
			}
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

	public void SetSelectionBox(Vector2 pos, Vector2 size)
	{
		selectionBox.position = pos;
		selectionBox.sizeDelta = size;
	}

	public void HideSelectionBox()
	{
		SetSelectionBox(new Vector2(-10, -10), Vector2.one);
	}

	public Vector3 FindMouseMapPos()
	{
		Vector3 mouseMapPoint = new Vector3();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Terrain")
		{
			mouseMapPoint = hit.point;
		}
		return mouseMapPoint;
	}

	public void RaycastSelect()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "PlayerUnit")
		{
			Selectable s = hit.transform.GetComponent<Selectable>();
			if (Input.GetKey(KeyCode.LeftShift))
				FlipSelection(s);
			else
				Select(s);
		}
	}

	public void BoxSelect()
	{
		Bounds b = new Bounds(selectionBox.position, selectionBox.sizeDelta);
		foreach (Selectable s in selectables)
		{

			if (b.Contains(s.screenPoint))
			{
				Select(s);
			}
		}
	}

	void FlipSelection(Selectable s)
	{
		if (!selection.Contains(s))
		{
			s.ChangeState(SelectionState.Selected);
			selection.Add(s);
		}
		else
		{
			s.ChangeState(SelectionState.NotSelected);
			selection.Remove(s);
		}
	}

	void Select(Selectable s)
	{
		if (!selection.Contains(s))
		{
			s.ChangeState(SelectionState.Selected);
			selection.Add(s);
		}
	}
	void Deselect(Selectable s)
	{
		if (selection.Contains(s))
		{
			s.ChangeState(SelectionState.NotSelected);
			selection.Remove(s);
		}
	}
	void DeselectAll()
	{
		foreach (Selectable s in selection)
			s.ChangeState(SelectionState.NotSelected);
		selection.Clear();
	}
	public void RotateCamera(Vector2 mouseInput)
	{
		transform.Rotate(Vector3.up, mouseInput.x * cameraRotSpeed * Time.deltaTime, Space.Self);
		cameraPivot.transform.Rotate(-Vector3.right, mouseInput.y * cameraRotSpeed * Time.deltaTime);
	}

	public void SetUnitDestinations()
	{
		foreach (Selectable s in selection)
		{
			s.GetComponent<Unit>().SetDestination(FindMouseMapPos());
		}
	}
}