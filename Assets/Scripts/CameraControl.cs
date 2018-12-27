using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
	public float zoom;
	public float targetZoom;
	public int zoomMin;
	public int zoomMax;
	public float zoomSpeed;
	public float cameraRotSpeed;

	public GameObject cameraPivot;

	public LayerMask mask;
	public Vector3 oldPos;

	public float smoothTime;
	public float rotSmoothTime;
	public float camRotSmoothTime;
	public float zoomSmoothTime;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rVelocity = Vector3.zero;
	private Vector3 cVelocity = Vector3.zero;
	private Vector3 zVelocity = Vector3.zero;

	public float camXRotMin;
	public float camXRotMax;

	public static CameraControl Instance;
	void Awake() { Instance = this; }

	void Update()
	{
		//Pivots camera based on mouse movement
		if (Input.GetMouseButton(1))
		{
			//transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * cameraRotSpeed * Time.deltaTime, Space.Self);
			Vector3 targetEuler = transform.eulerAngles + (Vector3.up * Input.GetAxis("Mouse X") * cameraRotSpeed);
			transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, targetEuler, ref rVelocity, rotSmoothTime);
			//cameraPivot.transform.Rotate(-Vector3.right, Input.GetAxis("Mouse Y") * cameraRotSpeed * Time.deltaTime);
			Vector3 camTargetEuler = cameraPivot.transform.eulerAngles + (-Vector3.right * Input.GetAxis("Mouse Y") * cameraRotSpeed);
			camTargetEuler = Vector3.SmoothDamp(cameraPivot.transform.eulerAngles, camTargetEuler, ref cVelocity, camRotSmoothTime);
			camTargetEuler.x = Mathf.Clamp(camTargetEuler.x, camXRotMin, camXRotMax);
			cameraPivot.transform.eulerAngles = camTargetEuler;
		}
		else
		{
			//Zoom camera
			zoom = -Camera.main.transform.localPosition.z;

			
				if (Camera.main.orthographic)
					Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
				else
				{
					targetZoom += -(Input.GetAxisRaw("Mouse ScrollWheel")) * zoomSpeed;
					if (targetZoom > zoomMax)
						targetZoom = zoomMax - 0.01f;
					if (targetZoom < zoomMin)
						targetZoom = zoomMin + 0.01f;
					Vector3 zoomV = new Vector3(0, 0, -targetZoom);
					Camera.main.transform.localPosition = Vector3.SmoothDamp(Camera.main.transform.localPosition, zoomV, ref zVelocity, zoomSmoothTime);
				}
			
		}
		
		if (Input.GetMouseButtonDown(2))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, mask))
				oldPos = hit.point;
		}
	}

	private void LateUpdate()
	{
		if (Input.GetMouseButton(2))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, mask))
			{
				Vector3 targetPosition = transform.position + (oldPos - hit.point);
				transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
			}
		}
	}
}