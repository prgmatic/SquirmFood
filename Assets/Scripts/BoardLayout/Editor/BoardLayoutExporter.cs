using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class BoardLayoutExporter
{
    [MenuItem("Monster Mashup/Export Board Layout", false, 1)]
    private static void CreateBoardLayout()
    {
        var savePath = EditorUtility.SaveFilePanel("Export Board Layout", "Assets", "Layout", "asset");

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
        return Application.isPlaying && Gameboard.Instance != null;
    }

    private static BoardLayout GenerateLayout()
    {
        BoardLayout layout = ScriptableObject.CreateInstance<BoardLayout>();
        foreach (var tile in Gameboard.Instance.gameTiles)
        {
            if (!tile.IsWorm)
            {
                try
                {
                    layout.Tokens.Add(new BoardLayout.TokenAtPoint(tile.TokenProperties, tile.GridPosition));
                }
                catch
                {
                    Debug.Log("oh");
                }
            }
        }

        return layout;
    }
}
