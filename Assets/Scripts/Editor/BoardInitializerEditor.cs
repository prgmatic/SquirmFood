using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections;

[CustomEditor(typeof(BoardInitalizer))]
public class BoardInitializerEditor : Editor 
{
    const int spacing = 3;

    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ReorderableListGUI.Title("Tokens To Spawn");
        ReorderableListGUI.ListField(((BoardInitalizer)target).TokensToSpawn, TokenSpawnDrawer, EditorGUIUtility.singleLineHeight + spacing * 2);
        serializedObject.ApplyModifiedProperties();
    }

    private BoardInitalizer.TokenWithWeight TokenSpawnDrawer(Rect position, BoardInitalizer.TokenWithWeight itemValue)
    {
        position.y += spacing;
        position.height = EditorGUIUtility.singleLineHeight;
        position.width = position.width / 2 - 3;
        itemValue.SpawnWeight = EditorGUI.IntSlider(position, itemValue.SpawnWeight, 0, 100);
        //position.y += EditorGUIUtility.singleLineHeight + spacing;
        position.width += 3;
        position.x += position.width;
        itemValue.Token = (Token)EditorGUI.ObjectField(position, itemValue.Token, typeof(Token), false);
        return itemValue;
    }
}
