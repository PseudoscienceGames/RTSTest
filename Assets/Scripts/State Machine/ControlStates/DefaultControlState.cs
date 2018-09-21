using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultControlState : ControlState
{
	private void Update()
	{
		ic.zoom = -Camera.main.transform.localPosition.z;
		if (ic.zoom > ic.zoomMin && ic.zoom < ic.zoomMax)
		{
			if (Camera.main.orthographic)
				Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * ic.zoomSpeed;
			else
			{
				ic.zoom += -(Input.GetAxisRaw("Mouse ScrollWheel")) * ic.zoomSpeed * ic.zoom;
				if (ic.zoom > ic.zoomMin && ic.zoom < ic.zoomMax)
				{
					Camera.main.transform.localPosition = new Vector3(0, 0, -ic.zoom);
				}
			}
		}
		if (!ic.mouseOverUI && Input.GetMouseButtonDown(0))
		{
			sm.ChangeState<ControlState0>();
		}
		if (!ic.mouseOverUI && Input.GetMouseButtonDown(1))
		{
			sm.ChangeState<ControlState1>();
		}
		if (!ic.mouseOverUI && Input.GetMouseButtonDown(2))
		{
			sm.ChangeState<ControlState2>();
		}
	}
}
