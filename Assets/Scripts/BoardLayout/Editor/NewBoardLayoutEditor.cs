using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NewBoardLayout))]
public class NewBoardLayoutEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var layout = (NewBoardLayout)target;
        
        layout.Difficulty = (NewBoardLayout.LevelDifficulty)EditorGUILayout.EnumPopup("Difficulty", layout.Difficulty);
        layout.NumberOfUndoes = EditorGUILayout.IntField("Number of Undoes", layout.NumberOfUndoes);
        layout.MudMask = (Sprite)EditorGUILayout.ObjectField("Mud Mask", layout.MudMask, typeof(Sprite), false);

        GUILayout.Label("Optional Goals");
        EditorGUI.indentLevel++;
        layout.LimitedMovesGoal = EditorGUILayout.Toggle("Limited Moves", layout.LimitedMovesGoal);
        if (layout.LimitedMovesGoal)
        {
            EditorGUI.indentLevel++;
            layout.NumberOfMoves = EditorGUILayout.IntField("Number of Moves", layout.NumberOfMoves);
            EditorGUI.indentLevel--;
        }
        layout.EatAllMushroomsGoal = EditorGUILayout.Toggle("Eat all mushrooms", layout.EatAllMushroomsGoal);
        EditorGUI.indentLevel--;

        EditorUtility.SetDirty(target);

        if (Application.isPlaying)
        {
            if(GUILayout.Button("Load"))
            {
                ((NewBoardLayout)target).Load();
            }
        }
    }
}
