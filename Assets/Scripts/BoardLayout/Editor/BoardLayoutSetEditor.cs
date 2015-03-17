using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using System.Collections.Generic;

[CustomEditor(typeof(BoardLayoutSet))]
public class BoardLayoutSetEditor : Editor
{
    const string _slothPunchKey = "54fee90bed334";
    List<BoardLayoutSet.ToggleableBoardLayout> _layouts = null;
    bool playing { get { return Application.isPlaying && Gameboard.Instance != null; } }
    private bool _syncing = false;
    private int _currentLayoutSync = 0;

    const int loadButtonWidth = 90;
    const int checkBoxWidth = 15;
    const int spacing = 3;

    void Awake()
    {
        _layouts = ((BoardLayoutSet)target).BoardLayouts;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ReorderableListGUI.Title("Board Layouts");
        float height = EditorGUIUtility.singleLineHeight + spacing * 2;
        if(playing) height = EditorGUIUtility.singleLineHeight * 2 + spacing * 3;
        ReorderableListGUI.ListField(_layouts, RecipeActionDrawer, height);

        if (!_syncing)
        {
            if (GUILayout.Button("Sync with Server"))
            {
                SyncLevelsWithServer();
            }
            if(GUILayout.Button("ReExport Levels"))
            {
                ReExportLevels();
            }
        }
        else
        {
            string nameOfCurrentLevel = _layouts[_currentLayoutSync].Layout.name;
            EditorUtility.DisplayProgressBar("Syncing Levels with Server", nameOfCurrentLevel, (float)_currentLayoutSync / _layouts.Count);
        }

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
        layout.Layout = (BoardLayout)EditorGUI.ObjectField(position, layout.Layout, typeof(BoardLayout), false);
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

    private void ReExportLevels()
    {
        string dir = "Assets/BoardLayouts/ReExports";
        if(!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }

        foreach(var layout in _layouts)
        {
            var data = BoardLayoutExporter.ExportBinary(layout.Layout);
            string path = dir + "/" + layout.Layout.name + ".asset";
            AssetDatabase.CreateAsset(BoardLayoutImporter.GetBoardLayoutFromBinary(data), path);
            AssetDatabase.Refresh();
        }
    }

    private void SyncLevelsWithServer()
    {
        if(_layouts.Count > 0)
        {
            _syncing = true;
            _currentLayoutSync = 0;
            // oh no!
            WebManager.Instance.SaveComplete += Insstance_SaveComplete;
            WebManager.Instance.SaveLevel(_layouts[_currentLayoutSync].Layout, _slothPunchKey);
        }
    }

    

    private void Insstance_SaveComplete(string levelName, int id)
    {
        _layouts[_currentLayoutSync].Layout.ID = id;
        EditorUtility.SetDirty(_layouts[_currentLayoutSync].Layout);
        _currentLayoutSync++;

        if(_currentLayoutSync < _layouts.Count)
        {
            WebManager.Instance.SaveLevel(_layouts[_currentLayoutSync].Layout, _slothPunchKey);
        }
        else
        {
            WebManager.Instance.SaveComplete -= Insstance_SaveComplete;
            _syncing = false;
            EditorUtility.ClearProgressBar();
        }
        Repaint();
    }
}
