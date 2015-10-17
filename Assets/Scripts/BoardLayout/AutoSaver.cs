#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class AutoSaver : MonoBehaviour
{

    public int SaveInterval = 60;
    private float _lastSave = 0;

    void Update()
    {
        if (Time.time - _lastSave >= SaveInterval)
        {
            SaveLevel(NewBoardLayout.FromGameboard(), "Autosave");
            _lastSave = Time.time;
        }
    }


    public static void SaveLevel(NewBoardLayout layout, string defaultName)
    {
        var path = "Assets/BoardLayouts/Autosave.asset";

        AssetDatabase.CreateAsset(layout, path);
        AssetDatabase.Refresh();

    }

}
#endif