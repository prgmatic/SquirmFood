using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;

[CustomEditor(typeof(MultiCondition))]
public class MultiConditionsEditor : Editor
{
    SerializedProperty conditions;
    
    void OnEnable()
    {
        conditions = serializedObject.FindProperty("Conditions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ReorderableListGUI.Title("Conditions");
        ReorderableListGUI.ListField(conditions);

        serializedObject.ApplyModifiedProperties();
    }
}
