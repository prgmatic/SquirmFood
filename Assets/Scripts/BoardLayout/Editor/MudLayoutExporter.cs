using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

public static class MudLayoutExporter
{
    private const string OutputDirectory = "MudMasks";
    private const int _width = 1400;
    private const int _height = 1680;
    private const int _leftOffset = 147;
    private const int _topOffset = 131;
    private const int _bottomOffset = 165;

    [MenuItem("Worm Food/Export Mud Layout")]
    public static void Export()
    {
        ExportMudLayout(NewBoardLayout.FromGameboard(), "test.png");
    }

    [MenuItem("Worm Food/Export All Mud Layouts")]
    private static void BatchExport()
    {
        var levels = GetAssets<NewBoardLayout>("BoardLayouts", "*.asset");
        for (int i = 0; i < levels.Length; i++)
        {
            var level = levels[i];
            EditorUtility.DisplayProgressBar("Exporting Mud Masks", level.name, (float)i / levels.Length);
            if (level.MudTiles.Count > 0)
                ExportMudLayout(level, OutputDirectory + "/" + level.name + ".png");
        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Worm Food/Auto Associate Mud Masks")]
    private static void AssociateMasks()
    {
        var levels = GetAssets<NewBoardLayout>("BoardLayouts", "*.asset");
        var masks = GetAssets<Sprite>("MudMasks", "*.png");

        for (int i = 0; i < levels.Length; i++)
        {
            var level = levels[i];
            EditorUtility.DisplayProgressBar("Exporting Mud Masks", level.name, (float)i / levels.Length);

            if (level.MudTiles.Count == 0) continue;
            foreach (var mask in masks)
            {
                if (level.name == mask.name)
                {
                    level.MudMask = mask;
                    EditorUtility.SetDirty(level);
                    break;
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    private static void ExportMudLayout(NewBoardLayout layout, string outputPath)
    {
        if (layout == null) return;
        Texture2D outputTexture = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
        outputTexture.SetPixels(0, 0, _width, _height, Color.clear);

        var tileWidth = (_width - _leftOffset * 2) / layout.Columns;
        var tileHeight = (_height - (_topOffset + _bottomOffset)) / layout.Rows;

        Color[] mudTileColors = new Color[tileWidth * tileHeight];
        for (int i = 0; i < mudTileColors.Length; i++)
            mudTileColors[i] = Color.white;


        foreach (var mudTile in layout.MudTiles)
        {
            int x = mudTile % layout.Columns;
            int y = mudTile / layout.Columns;
            outputTexture.SetPixels(x * tileWidth + _leftOffset, _height - (y + 1) * tileHeight - _topOffset, tileWidth, tileHeight, mudTileColors);
        }

        outputTexture.Apply();

        byte[] data = outputTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + outputPath, data);
    }

    private static void SetPixels(this Texture2D texture, int x, int y, int width, int height, Color color)
    {
        var colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture.SetPixels(x, y, width, height, colors);
    }

    private static string[] GetAssetPaths(string directory, string searchOptions)
    {
        return Directory.GetFiles(Application.dataPath + "/" + directory, searchOptions, SearchOption.AllDirectories);
    }

    private static T[] GetAssets<T>(string directory, string searchOptions) where T : UnityEngine.Object
    {
        var results = new List<T>();
        var files = GetAssetPaths(directory, searchOptions);
        if (!Directory.Exists(Application.dataPath + "/" + OutputDirectory))
            Directory.CreateDirectory(Application.dataPath + "/" + OutputDirectory);

        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            var path = file.Substring(Application.dataPath.Length - 6, file.Length - (Application.dataPath.Length - 6));
            path = path.Replace("\\", "/");

            var asset = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
            if (asset != null)
            {
                results.Add(asset);
            }
        }
        return results.ToArray();
    }
}
