using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class PipeLayoutExporter
{
    [MenuItem("Worm Food/Export Pipe Layout")]
    public static void ExportPipeLayout()
    {
        NewBoardLayout layout = NewBoardLayout.FromGameboard();
        if (layout == null) return;

        string data = "";
        foreach (var tileInfo in layout.Tiles)
        {
            var tile = NewBoardLayout.GetPrefab(tileInfo.ID);
            if (!tile.Pushable && !tile.IsEdible && !tile.CanFall && !tile.IsWorm)
            {
                float rotation;
                var pipeName = PipeObject.GetPipeNameAndRotation(layout, tileInfo.X, tileInfo.Y, out rotation);
                if (data.Length > 0) data += ",";
                data += string.Format("{0},{1},{2},{3}", pipeName, rotation, tileInfo.X, tileInfo.Y);
            }
        }
        File.WriteAllText(Application.dataPath + @"\test.pipeLayout", data);
    }
}
