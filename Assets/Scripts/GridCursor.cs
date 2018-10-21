using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCursor : MonoBehaviour
{
	private void Update()
	{
		Vector3 pos = InputController.instance.FindMouseMapPos();
		pos.x = Mathf.RoundToInt(pos.x);
		pos.z = Mathf.RoundToInt(pos.z);
		transform.position = pos;
	}
}
