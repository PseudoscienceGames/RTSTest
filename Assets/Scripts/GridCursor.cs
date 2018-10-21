using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCursor : MonoBehaviour
{
	public static GridCursor instance;
	public Vector3Int gridLoc;
	void Awake() { instance = this; }

	private void Update()
	{
		Vector3 pos = InputController.instance.FindMouseMapPos();
		gridLoc.x = Mathf.RoundToInt(pos.x);
		gridLoc.y = Mathf.RoundToInt(pos.y);
		gridLoc.z = Mathf.RoundToInt(pos.z);
		pos = gridLoc;
	
		transform.position = pos;
	}
}
