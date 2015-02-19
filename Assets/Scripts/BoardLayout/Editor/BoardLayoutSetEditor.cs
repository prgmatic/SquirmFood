using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections.Generic;

[CustomEditor(typeof(BoardLayoutSet))]
public class BoardLayoutSetEditor : Editor
{
    bool playing { get { return Application.isPlaying && Gameboard.Instance != null; } }

    const int loadButtonWidth = 90;
    const int checkBoxWidth = 15;
    const int spacing = 3;
    SerializedProperty layouts;
    SerializedProperty testerName;
    SerializedProperty autoLog;


    void OnEnable()
    {
        layouts = serializedObject.FindProperty("BoardLayouts");
        testerName = serializedObject.FindProperty("PlayTesterName");
        autoLog = serializedObject.FindProperty("AutoLogPlaytest");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        testerName.stringValue = EditorGUILayout.TextField("Play Tester Name", testerName.stringValue);
        autoLog.boolValue = EditorGUILayout.Toggle("Auto Log Playthrough", autoLog.boolValue);

        ReorderableListGUI.Title("Board Layouts");
        float height = EditorGUIUtility.singleLineHeight + spacing * 2;
        if(playing) height = EditorGUIUtility.singleLineHeight * 2 + spacing * 3;
        ReorderableListGUI.ListField(((BoardLayoutSet)target).BoardLayouts, RecipeActionDrawer, height);

        serializedObject.ApplyModifiedProperties();
    }

    private BoardLayoutSet.ToggleableBoardLayout RecipeActionDrawer(Rect position, BoardLayoutSet.ToggleableBoardLayout layout)
    {
        float y = position.y;
        float width = position.width;
        float height = position.height;

        position.height = EditorGUIUtility.singleLineHeight;
        position.y += height / 2 - position.height / 2;
        position.width = checkBoxWidth;
        layout.Enabled = EditorGUI.Toggle(position, layout.Enabled);


        position.x += checkBoxWidth + spacing;
        position.width = width - checkBoxWidth - spacing;
        if(playing)
            position.width = width - checkBoxWidth - loadButtonWidth - spacing * 2;
        layout.Layout = (BoardLayout)EditorGUI.ObjectField(position, layout.Layout, typeof(BoardLayout));
        position.x += position.width + spacing;
        position.width = loadButtonWidth;
        //position.height = height;
        position.y = y + spacing;
        if (playing)
        {
            if (GUI.Button(position, "Load"))
            {
                ((BoardLayoutSet)target).SetLayout(layout.Layout);
                Gameboard.Instance.StartGame();
            }
            position.y += EditorGUIUtility.singleLineHeight + spacing;
            if (GUI.Button(position, "Playthroughs"))
            {
                var window = (PlaythroughsWindow)EditorWindow.GetWindow(typeof(PlaythroughsWindow));
                window.SetLayout(layout.Layout);
            }
        }
        else
        {
            /*
            if(GUI.Button(position, "Playthroughs"))
            {
                var window = (PlaythroughsWindow) EditorWindow.GetWindow(typeof(PlaythroughsWindow));
                window.SetLayout(layout.Layout);
            }
            */
        }

        return layout;
    }

}
