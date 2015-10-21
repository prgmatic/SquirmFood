using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NewBoardLayout))]
public class NewBoardLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var layout = (NewBoardLayout)target;
        layout.Difficulty = (NewBoardLayout.LevelDifficulty)EditorGUILayout.EnumPopup(layout.Difficulty);
        if(Application.isPlaying)
        {
            if(GUILayout.Button("Load"))
            {
                ((NewBoardLayout)target).Load();
            }
        }
    }
}
