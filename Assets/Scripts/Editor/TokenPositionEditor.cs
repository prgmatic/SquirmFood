using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TokenPositioner))]
public class TokenPositionEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("Move"))
		{
			var tp = (TokenPositioner)target;

			tp.transform.position = GridPosToWorldPosition(tp.X, tp.Y);
        }
	}

	private Vector3 GridPosToWorldPosition(int x, int y)
	{
		float gridWidth = 8;
		float gridHeight = 10;
		float xPos = ((float)x - gridWidth / 2) + 0.5f;
		float yPos = ((float)y - gridHeight / 2) + 0.5f;
		return new Vector3(xPos, yPos, 0f);
	}
}
