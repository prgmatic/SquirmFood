using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(VictoryLossConditions))]
public class VictoryLossConditionsEditor : Editor 
{
    SerializedProperty victories;
    SerializedProperty losses;

    void OnEnable()
    {
        victories = serializedObject.FindProperty("VictoryConditions");
        losses = serializedObject.FindProperty("LossConditions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ReorderableListGUI.Title("Victory Conditions");
        ReorderableListGUI.ListField(victories);

        ReorderableListGUI.Title("Loss Conditions");
        ReorderableListGUI.ListField(losses);

        serializedObject.ApplyModifiedProperties();
    }
}
