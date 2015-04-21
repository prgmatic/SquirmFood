using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CameraTest : MonoBehaviour 
{

	public Camera PerspectiveCamera;
	public float Offset = 0f;
	private Camera _orthoCamera;
	

	void Awake()
	{
		_orthoCamera = GetComponent<Camera>();
	}

	float FrustumHeightAtDistance(float distance)
	{
		return 2.0f * distance * Mathf.Tan(PerspectiveCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
	}


	void Update()
	{
		if (Application.isPlaying)
			UpdateOrthoSize();
	}

	void OnRenderObject()
	{
		if (!Application.isPlaying)
			UpdateOrthoSize();
	}

	void UpdateOrthoSize()
	{
		if (PerspectiveCamera != null)
		{
			var ray = new Ray(PerspectiveCamera.transform.position, PerspectiveCamera.transform.forward);
			float rayDistance;
			if (Utils.WorldPlane.Raycast(ray, out rayDistance))
			{
				float frustrumHeight = FrustumHeightAtDistance(rayDistance);
				_orthoCamera.orthographicSize = frustrumHeight / 2 * Offset;
			}
		}
	}
}
