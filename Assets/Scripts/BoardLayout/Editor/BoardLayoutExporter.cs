using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BoardLayoutExporter
{
    private static bool CanExport { get { return Application.isPlaying && Gameboard.Instance != null; } }
    private static List<BoardLayout.TokenAtPoint> quickSaveTokens = new List<BoardLayout.TokenAtPoint>();

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
            AssetDatabase.CreateAsset(GenerateLayout(), savePath);
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
        quickSaveTokens = GetTokensOnBoard();
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
        ImportBoardLayout(quickSaveTokens);
        Debug.Log("Layout Quick Import");
    }
    [MenuItem("Monster Mashup/Quick Import _&a", true)]
    public static bool CanQuickImport()
    {
        return CanExport;
    }



    public static List<BoardLayout.TokenAtPoint> GetTokensOnBoard()
    {
        List<BoardLayout.TokenAtPoint> result = new List<BoardLayout.TokenAtPoint>();
        foreach (var tile in Gameboard.Instance.gameTiles)
        {
            if (!tile.IsWorm || tile.GetComponent<Worm>() != null)
            {
                result.Add(new BoardLayout.TokenAtPoint(tile.TokenProperties, tile.GridPosition));
            }
        }
        return result;
    }

    private static BoardLayout GenerateLayout()
    {
        BoardLayout layout = ScriptableObject.CreateInstance<BoardLayout>();
        layout.Tokens = GetTokensOnBoard();
        return layout;
    }

    public static void ImportBoardLayout(List<BoardLayout.TokenAtPoint> tokens)
    {
        WormSpawnerInput wormSpawner = Gameboard.Instance.GetComponent<WormSpawnerInput>();

        Gameboard.Instance.Clear();
        foreach (var token in tokens)
        {
            if (token.Token.IsWorm)
            {
                if (wormSpawner != null)
                    wormSpawner.CreateWorm(token.Position);
            }
            else
                Gameboard.Instance.AddTileFromToken(token.Token, token.Position, false, true);
        }
        Gameboard.Instance.ApplyGravity();
    }
}
