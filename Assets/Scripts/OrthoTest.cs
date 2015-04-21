using UnityEngine;
using System.Collections.Generic;

public class OrthoTest : MonoBehaviour 
{

	public Camera _camera;
	void Update()
	{
		//Debug.Log(  projection.MultiplyVector
	}

	void OnDrawGizmos()
	{
		var projection = _camera.projectionMatrix;
		var view = Matrix4x4.TRS(_camera.transform.position, _camera.transform.rotation, Vector3.one);
		//var v = Camera.main.WorldToViewportPoint
		var pos = view.MultiplyPoint(this.transform.position);
		//var m = projection * view;
		pos = projection.MultiplyPoint(pos);
		//pos = view.inverse.MultiplyPoint(pos);
		//pos.z = 0f;

		//float4 viewPosition = mul(pos, View);
		//output.Position = mul(viewPosition, Projection);
		Debug.Log(pos);
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(pos, Vector3.one / 2f);
	}
}
