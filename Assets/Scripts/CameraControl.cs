using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
	public float zoom;
	public int zoomMin;
	public int zoomMax;
	public float zoomSpeed;
	public float cameraRotSpeed;
	public float camPanSpeed;
	public int scrollAreaSize;
	public bool screenEdgeScroll;

	public GameObject cameraPivot;

	public static CameraControl Instance;
	void Awake() { Instance = this; }


	void Update()
	{
		if (Input.GetAxis("Vertical") != 0)
			transform.position += transform.forward * Input.GetAxisRaw("Vertical") * camPanSpeed;
		if (Input.GetAxis("Horizontal") != 0)
			transform.position += transform.right * Input.GetAxisRaw("Horizontal") * camPanSpeed;
		//Pivots camera based on mouse movement
		if (Input.GetMouseButton(1))
		{
			transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * cameraRotSpeed * Time.deltaTime, Space.Self);
			cameraPivot.transform.Rotate(-Vector3.right, Input.GetAxis("Mouse Y") * cameraRotSpeed * Time.deltaTime);
		}
		else if (screenEdgeScroll)
		{
			if (Input.mousePosition.x < scrollAreaSize)
				transform.position -= transform.right * camPanSpeed;
			if (Input.mousePosition.x > Camera.main.pixelWidth - scrollAreaSize)
				transform.position += transform.right * camPanSpeed;
			if (Input.mousePosition.y < scrollAreaSize)
				transform.position -= transform.forward * camPanSpeed;
			if (Input.mousePosition.y > Camera.main.pixelHeight - scrollAreaSize)
				transform.position += transform.forward * camPanSpeed;
		}
		if (Input.GetMouseButton(2))
			transform.position -= transform.TransformVector(new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")));

		//Zoom camera
		zoom = -Camera.main.transform.localPosition.z;
		if (zoom > zoomMin && zoom < zoomMax)
		{
			if (Camera.main.orthographic)
				Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
			else
			{
				zoom += -(Input.GetAxisRaw("Mouse ScrollWheel")) * zoomSpeed * zoom;
				if (zoom > zoomMin && zoom < zoomMax)
				{
					Camera.main.transform.localPosition = new Vector3(0, 0, -zoom);
				}
			}
		}

		//public void FocusCamera(Vector2 gridLoc)
		//{
		//	StopAllCoroutines();
		//	StartCoroutine(SmoothFocus(gridLoc));
		//}

		//IEnumerator SmoothFocus(Vector2 gridLoc)
		//{
		//	Vector3 initPos = transform.position;
		//	Vector3 targetPos = Grid.GridToWorld(gridLoc, 0);

		//	float timer = 0;
		//	while (timer <= 1)
		//	{
		//		timer += Time.deltaTime;
		//		transform.position = Vector3.Lerp(initPos, targetPos, timer);
		//		yield return null;
		//	}
		//}
	}
}