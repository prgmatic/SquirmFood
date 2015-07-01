using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class PipeLayoutExporter
{
    [MenuItem("Worm Food/Export Pipe Layout")]
    public static void ExportPipeLayout()
    {
        BoardLayout layout = BoardLayoutExporter.GenerateLayout();
        if (layout == null) return;

        string data = "";
        foreach (var token in layout.Tokens)
        {

            if (!token.Token.Pushable && !token.Token.IsEdible && !token.Token.CanFall && !token.Token.IsWorm)
            {
                float rotation;
                var pipeName = PipeObject.GetPipeNameAndRotation(layout, token.Position.x, token.Position.y, out rotation);
                if (data.Length > 0) data += ",";
                data += string.Format("{0},{1},{2},{3}", pipeName, rotation, token.Position.x, token.Position.y);
            }
        }
        File.WriteAllText(Application.dataPath + @"\test.pipeLayout", data);
    }
}
