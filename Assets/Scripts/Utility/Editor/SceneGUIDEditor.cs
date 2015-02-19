using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SceneGUID))]
public class SceneGUIDEditor : Editor
{
    SerializedProperty guid;

    void OnEnable()
    {
        guid = serializedObject.FindProperty("GUID");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        guid.stringValue = AssetDatabase.AssetPathToGUID(EditorApplication.currentScene);
        GUILayout.Label(string.Format("GUID: {0}", guid.stringValue));
        serializedObject.ApplyModifiedProperties();
    }

}
