using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections.Generic;

[CustomEditor(typeof(LevelSet))]
public class LevelSetEditor : Editor 
{
	private SerializedProperty levels;


	void OnEnable()
	{
		levels = serializedObject.FindProperty("Levels");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		ReorderableListGUI.Title("Levels");
		ReorderableListGUI.ListField(levels);
		serializedObject.ApplyModifiedProperties();
	}
}
