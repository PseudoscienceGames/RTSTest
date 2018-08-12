using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMarker : MonoBehaviour
{
	public Transform unit;
	public RectTransform rt;

	private void Start()
	{
		rt = GetComponent<RectTransform>();
	}

	private void Update()
	{
		Bounds b = unit.GetChild(0).GetComponent<Renderer>().bounds;
		List<Vector2> screenPoints = new List<Vector2>();
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.min.x, b.min.y, b.min.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.min.x, b.min.y, b.max.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.min.x, b.max.y, b.max.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.min.x, b.max.y, b.min.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.max.x, b.min.y, b.min.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.max.x, b.min.y, b.max.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.max.x, b.max.y, b.min.z)));
		screenPoints.Add(Camera.main.WorldToScreenPoint(new Vector3(b.max.x, b.max.y, b.max.z)));
		Vector2 min = Camera.main.WorldToScreenPoint(b.center);
		Vector2 max = Camera.main.WorldToScreenPoint(b.center);
		foreach (Vector2 p in screenPoints)
		{
			min = Vector2.Min(min, p);
			max = Vector2.Max(max, p);
			min -= Vector2.one * 0.1f;
			max += Vector2.one * 0.1f;
		}
		rt.position = (min + max) / 2f;
		rt.sizeDelta = 2 * ((Vector2)rt.position - min);
		rt.sizeDelta = new Vector2(rt.sizeDelta.x * 1.1f, rt.sizeDelta.y * 1.05f);
	}

	public void Highlight()
	{
		GetComponent<Image>().enabled = true;
	}

	public void Select()
	{
		GetComponent<Image>().enabled = true;
	}

	public void Hide()
	{
		GetComponent<Image>().enabled = false;
	}
}
