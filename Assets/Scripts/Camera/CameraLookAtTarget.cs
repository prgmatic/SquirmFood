using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CameraLookAtTarget : MonoBehaviour 
{
	private Camera _camera;
	public Transform Target;

	void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	void Update()
	{
		if (Target != null)
		{
			_camera.transform.LookAt(Target);
		}
	}
}
