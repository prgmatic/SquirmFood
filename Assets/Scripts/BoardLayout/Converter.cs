using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class Converter : MonoBehaviour
{

    //[MenuItem("Worm Food/Convert levels to new format")]
    //public static void Convert()
    //{
    //    string path = @"BoardLayouts/PushPuzzles/LimitedMove";
    //    string newPath = "Assets/BoardLayouts/PushPuzzles/NewLayout";
    //    //Debug.Log(Directory.Exists(path) ? "found it" : "nope");
    //    //var files = Directory.GetFiles(path, "*.asset");

    //    var layouts = GetAtPath<BoardLayout>(path);

    //    foreach (var layout in layouts)
    //    {
    //        var newLayout = NewBoardLayout.FromOldBoardLayout(layout);

    //        AssetDatabase.CreateAsset(newLayout, newPath + "/" + layout.name + ".asset");
    //    }
    //    AssetDatabase.Refresh();
    //}
    public static T[] GetAtPath<T>(string path)
    {

        ArrayList al = new ArrayList();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

        foreach (string fileName in fileEntries)
        {
            int assetPathIndex = fileName.IndexOf("Assets");
            string localPath = fileName.Substring(assetPathIndex);
            localPath = localPath.Replace("\\", "/");

            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

            if (t != null)
                al.Add(t);
        }
        T[] result = new T[al.Count];
        for (int i = 0; i < al.Count; i++)
            result[i] = (T)al[i];

        return result;
    }




}
