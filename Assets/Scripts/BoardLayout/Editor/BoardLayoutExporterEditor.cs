using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BoardLayoutExporterEditor
{
    private static bool CanExport { get { return Application.isPlaying && Gameboard.Instance != null; } }
    private static BoardLayout QuickSaveLayout = null;

    [MenuItem("Monster Mashup/Export Board Layout", false, 1)]
    private static void CreateBoardLayout()
    {
        string startingDir = "Assets";
        if (Directory.Exists(startingDir + "/BoardLayouts")) startingDir += "/BoardLayouts";
        var savePath = EditorUtility.SaveFilePanel("Export Board Layout", startingDir, "Layout", "asset");
        if (savePath.ToLower().StartsWith(Application.dataPath.ToLower()))
        {
            savePath = savePath.Substring(Application.dataPath.Length, savePath.Length - Application.dataPath.Length);
            savePath = @"Assets" + savePath;
            AssetDatabase.CreateAsset(BoardLayoutExporter.GenerateLayout(), savePath);
            AssetDatabase.Refresh();
            Debug.Log("Layout Saved");
        }
        else
        {
            Debug.LogError("Layout must be exported to the projects assets folder.");
        }
    }

    [MenuItem("Monster Mashup/Export Board Layout", true)]
    private static bool CanExportBoardLayout()
    {
        return CanExport;
    }

    [MenuItem("Monster Mashup/Quick Export _&s", false, 1)]
    public static void QuickExport()
    {
        QuickSaveLayout = BoardLayoutExporter.GenerateLayout();
        Debug.Log("Layout Quick Export");
    }
    [MenuItem("Monster Mashup/Quick Export _&s", true)]
    public static bool CanQuickExport()
    {
        return CanExport;
    }
    [MenuItem("Monster Mashup/Quick Import _&a", false, 1)]
    public static void QuickImport()
    {
        if (QuickSaveLayout != null)
        {
            BoardLayoutImporter.ImportBoardLayout(QuickSaveLayout);
            Debug.Log("Layout Quick Import");
        }
    }
    [MenuItem("Monster Mashup/Quick Import _&a", true)]
    public static bool CanQuickImport()
    {
        return CanExport;
    }



    
}
