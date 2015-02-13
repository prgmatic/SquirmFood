using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(MultiAction))]
public class MultiActionEditor : Editor 
{
    SerializedProperty actions;

    void OnEnable()
    {
        actions = serializedObject.FindProperty("Actions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ReorderableListGUI.Title("Actions");
        ReorderableListGUI.ListField(actions);

        serializedObject.ApplyModifiedProperties();
    }
}
