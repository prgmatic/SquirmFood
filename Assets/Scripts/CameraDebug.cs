using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CameraDebug : MonoBehaviour 
{

	private Camera _camera;

	void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	float FrustumHeightAtDistance(float distance)
	{
		return 2.0f * distance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
	}

    void OnGUI()
	{
		/*
		var ray = new Ray(_camera.transform.position, _camera.transform.forward);
		float rayDistance;
		if (Utils.WorldPlane.Raycast(ray, out rayDistance))
		{
			float frustumHeight = FrustumHeightAtDistance(rayDistance);
			GUILayout.Label(string.Format("Frustum Height: {0}", frustumHeight));
			GUILayout.Label(string.Format("Frustum Width: {0}", frustumHeight * _camera.aspect));
			GUILayout.Label(string.Format("Camera Aspect: {0}", _camera.aspect));
		}
		*/
	}
}
