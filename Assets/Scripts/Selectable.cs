using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
	public Vector2 screenPoint;

	private void Update()
	{
		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
	}

	void Start ()
	{
		GameObject.Find("Selector").GetComponent<Selector>().selectables.Add(this);
	}
}
