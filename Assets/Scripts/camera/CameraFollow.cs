using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject followTarget;

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	private float cameraZ = 0;

	private Camera cam = null;

	const float CAM_VIEW_SIZE = 5;

	void Start()
	{
		cameraZ = transform.position.z;
		cam = GetComponent<Camera>();
	}

	void FixedUpdate()
	{
		if (followTarget)
		{
			Vector3 delta = followTarget.transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cameraZ));
			Vector3 destination = transform.position + delta;
			destination.z = cameraZ;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}

	public void SetCameraTarger(GameObject target)
	{
		followTarget = target;
	}

	public void ZoomCamera(bool isZoomOut, float zoomStep, float maxZoom = CAM_VIEW_SIZE) 
	{
		if (isZoomOut == false)
		{
			StartCoroutine(ZoomIn(maxZoom, zoomStep));
		}
		else 
		{
			StartCoroutine(ZoomOut(maxZoom, zoomStep));
		}
	}

	IEnumerator ZoomIn(float maxZoom, float zoomStep) 
	{
		while (cam.orthographicSize > maxZoom) 
		{
			cam.orthographicSize -= zoomStep;
			yield return new WaitForSeconds(0.01f);
		}
		//cam.orthographicSize = maxZoom;
	}

	IEnumerator ZoomOut(float maxZoom, float zoomStep)
	{
		while (cam.orthographicSize < maxZoom)
		{
			cam.orthographicSize += zoomStep;
			yield return new WaitForSeconds(0.01f);
		}
		cam.orthographicSize = maxZoom;
	}
}
