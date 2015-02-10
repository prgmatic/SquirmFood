using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(BoardInitalizer))]
public class BoardInitializerEditor : Editor 
{
    const int spacing = 3;
    SerializedProperty _tokensToSpawnProp;

    void OnEnable()
    {
        _tokensToSpawnProp = serializedObject.FindProperty("TokensToSpawn");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ReorderableListGUI.Title("Tokens To Spawn");
        ReorderableListGUI.ListField(((BoardInitalizer)target).TokensToSpawn, TokenSpawnDrawer, EditorGUIUtility.singleLineHeight * 2 + spacing + 3);
        serializedObject.ApplyModifiedProperties();
    }

    private BoardInitalizer.TokenWithWeight TokenSpawnDrawer(Rect position, BoardInitalizer.TokenWithWeight itemValue)
    {
        position.y += spacing;
        position.height = EditorGUIUtility.singleLineHeight;
        itemValue.Token = (Token)EditorGUI.ObjectField(position, "Token", itemValue.Token, typeof(Token));
        position.y += EditorGUIUtility.singleLineHeight + spacing;
        itemValue.SpawnWeight = EditorGUI.IntSlider(position, "Spawn Weight", itemValue.SpawnWeight, 0, 100);
        return itemValue;
    }
}
