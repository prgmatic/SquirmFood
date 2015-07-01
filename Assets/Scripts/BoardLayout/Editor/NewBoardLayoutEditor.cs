using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NewBoardLayout))]
public class NewBoardLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(Application.isPlaying)
        {
            if(GUILayout.Button("Load"))
            {
                ((NewBoardLayout)target).Load();
            }
        }
    }
}
