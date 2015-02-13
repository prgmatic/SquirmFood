using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PathFinder))]
public class PathFinderEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying && Gameboard.Instance != null)
        {
            if (GUILayout.Button("Find Path"))
            {
                ((PathFinder)target).FindPath();
            }
        }
    }
}
