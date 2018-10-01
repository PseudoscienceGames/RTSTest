using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMarker : MonoBehaviour
{
	public Transform unit;
	public RectTransform rt;
	public Renderer r;

	private void Start()
	{
		rt = GetComponent<RectTransform>();
	}

	private void Update()
	{
		Bounds b = r.bounds;
		List<Vector2> screenPoints = new List<Vector2>();
		Vector3 cross = Vector3.Cross(Vector3.up, b.center - Camera.main.transform.position).normalized * 0.5f;
		screenPoints.Add(Camera.main.WorldToScreenPoint(b.center + Vector3.up + cross));
		screenPoints.Add(Camera.main.WorldToScreenPoint(b.center + Vector3.up - cross));
		screenPoints.Add(Camera.main.WorldToScreenPoint(b.center - Vector3.up + cross));
		screenPoints.Add(Camera.main.WorldToScreenPoint(b.center - Vector3.up - cross));
		Vector2 min = Camera.main.WorldToScreenPoint(b.center);
		Vector2 max = Camera.main.WorldToScreenPoint(b.center);
		foreach (Vector2 p in screenPoints)
		{
			min = Vector2.Min(min, p);
			max = Vector2.Max(max, p);
		}
		rt.position = (min + max) / 2f;
		rt.sizeDelta = 2 * ((Vector2)rt.position - min);
		rt.sizeDelta = new Vector2(rt.sizeDelta.x * .95f, rt.sizeDelta.y * .95f);

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