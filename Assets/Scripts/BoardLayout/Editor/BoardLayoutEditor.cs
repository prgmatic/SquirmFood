using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BoardLayout))]
public class BoardLayoutEditor : Editor
{
    SerializedProperty guid;

    void OnEnable()
    {
        guid = serializedObject.FindProperty("GUID");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        guid.stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath((BoardLayout)serializedObject.targetObject));
        GUILayout.Label("GUID: " + guid.stringValue);
        serializedObject.ApplyModifiedProperties();
    }
}
