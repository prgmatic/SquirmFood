using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(DebugTileSpawner))]
public class DebugSpawnerEditor : Editor 
{
    SerializedProperty _tokenToSpawnProp;
    void OnEnable()
    {
        _tokenToSpawnProp = serializedObject.FindProperty("TokensToSpawn");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ReorderableListGUI.Title("Tiles to Spawn");
        ReorderableListGUI.ListField(_tokenToSpawnProp);

        serializedObject.ApplyModifiedProperties();
    }
}
