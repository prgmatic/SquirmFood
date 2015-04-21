using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PerspectiveCameraAdjustment : MonoBehaviour 
{

	[Range(1f, 179f)]
	public float FieldOfView = 45f;
	public float TargetFrustrumWidth = 9.5f;

	private Camera _camera;
	private float _distance = 0f;
	
	void Awake()
	{
		_camera = GetComponent<Camera>();
		_distance = GetDistance();
	}
	void Update()
	{
		if(Application.isPlaying)
		{
			UpdateFOV();
		}
	}

	void OnRenderObject()
	{
		if(!Application.isPlaying)
		{
			UpdateFOV();
		}
	}

	void UpdateFOV()
	{
		var frustumHeight = FrustumHeightAtDistance(_distance, FieldOfView);
		var frustumWidth = frustumHeight * _camera.aspect;
		if (frustumWidth < TargetFrustrumWidth)
		{
			var targetHeight = TargetFrustrumWidth / frustumWidth * frustumHeight;
			_camera.fieldOfView = FOVForHeightAndDistance(targetHeight, _distance);
		}
		else
		{
			_camera.fieldOfView = FieldOfView;
		}
	}

	private float GetDistance()
	{
		var ray = new Ray(_camera.transform.position, _camera.transform.forward);
		float rayDistance;
		if (Utils.WorldPlane.Raycast(ray, out rayDistance))
		{
			return rayDistance;
		}
		return 0f;
	}
	private float FrustumHeightAtDistance(float distance, float fov)
	{
		return 2.0f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
	}
	private float FOVForHeightAndDistance(float height, float distance)
	{
		return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
	}
}
