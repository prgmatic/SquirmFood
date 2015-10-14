using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class LayoutUtils : MonoBehaviour
{

    private static bool CanExport { get { return Application.isPlaying && Gameboard.Instance != null; } }

    private static byte[] QuickSaveData = null;

    [MenuItem("Worm Food/Export Board Layout", false, 15)]
    private static void CreateBoardLayout()
    {
        SaveLevel(NewBoardLayout.FromGameboard(), "Layout");
    }

    public static void SaveLevel(NewBoardLayout layout, string defaultName)
    {
        string startingDir = "Assets";
        if (Directory.Exists(startingDir + "/BoardLayouts")) startingDir += "/BoardLayouts";
        var savePath = EditorUtility.SaveFilePanel("Save Board Layout", startingDir, defaultName, "asset");
        if (savePath.Length > 0 && savePath.ToLower().StartsWith(Application.dataPath.ToLower()))
        {
            savePath = savePath.Substring(Application.dataPath.Length, savePath.Length - Application.dataPath.Length);
            savePath = @"Assets" + savePath;
            AssetDatabase.CreateAsset(layout, savePath);
            AssetDatabase.Refresh();
            Debug.Log("Layout Saved");
        }
        else
        {
            Debug.LogError("Layout must be exported to the projects assets folder.");
        }
    }

    [MenuItem("Worm Food/Export Board Layout", true)]
    private static bool CanExportBoardLayout()
    {
        return CanExport;
    }

    [MenuItem("Worm Food/Quick Export _&s", false, 15)]
    public static void QuickExport()
    {
        QuickSaveData = NewBoardLayout.FromGameboard().ToBinary();
        Debug.Log("Layout Quick Export");
    }
    [MenuItem("Worm Food/Quick Export _&s", true)]
    public static bool CanQuickExport()
    {
        return CanExport;
    }
    [MenuItem("Worm Food/Quick Import _&a", false, 15)]
    public static void QuickImport()
    {
        if (QuickSaveData != null)
        {
            NewBoardLayout.FromBinary(QuickSaveData).Load();
            Debug.Log("Layout Quick Import");
        }
    }
    [MenuItem("Worm Food/Quick Import _&a", true)]
    public static bool CanQuickImport()
    {
        return CanExport;
    }
}
