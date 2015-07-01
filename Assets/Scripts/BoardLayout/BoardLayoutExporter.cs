using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class BoardLayoutExporter 
{
    const int ExportVersion = 1;

    public static List<BoardLayout.TokenAtPoint> GetTokensOnBoard()
    {
        List<BoardLayout.TokenAtPoint> result = new List<BoardLayout.TokenAtPoint>();
        foreach (var tile in Gameboard.Instance.gameTiles)
        {
            if (!tile.IsWorm || tile.GetComponent<Worm>() != null)
            {
                //result.Add(new BoardLayout.TokenAtPoint(tile.TokenProperties, tile.GridPosition, tile.Variation));
            }
        }
        return result;
    }

    public static BoardLayout GenerateLayout()
    {
        BoardLayout layout = ScriptableObject.CreateInstance<BoardLayout>();
        layout.Tokens = GetTokensOnBoard();
        layout.Columns = Gameboard.Instance.Columns;
        layout.Rows = Gameboard.Instance.Rows;
        layout.BackgroundTileAttributes = new Gameboard.BackgroundTileAttribute[layout.Columns * layout.Rows];
        for(int y = 0; y < layout.Rows; y++)
        {
            for(int x = 0; x < layout.Columns; x++)
            {
                layout.BackgroundTileAttributes[x + y * layout.Columns] = Gameboard.Instance.GetBackgroundTileAttribute(x, y);
            }
        }
        //layout.BackgroundTileAttributes = Gameboard.Instance.ExportBackgroundTileAtributes();
        return layout;
    }

    public static byte[] ExportBinary()
    {
        return ExportBinary(GenerateLayout());
    }

    public static byte[] ExportBinary(BoardLayout layout)
    {
        byte[] result = null;
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                //Gameboard gb = Gameboard.Instance;

                writer.Write(ExportVersion);

                writer.Write(layout.Columns);
                writer.Write(layout.Rows);

                for (int y = 0; y < layout.Rows; y++)
                {
                    for (int x = 0; x < layout.Columns; x++)
                    {
                        if (layout.BackgroundTileAttributes.Length > x + y * layout.Columns)
                            writer.Write(layout.BackgroundTileAttributes[x + y * layout.Columns] == Gameboard.BackgroundTileAttribute.FreeMove);
                        else
                            writer.Write(false);
                    }
                }
                writer.Write(layout.Tokens.Count);
                foreach(var token in layout.Tokens)
                {
                    if (token.Token.ID == 0 || token.Token.ID == 255)
                        Debug.LogError("Token " + token.Token.name + " does not have an ID");
                    writer.Write(token.Token.ID);
                    writer.Write((byte)token.Position.x);
                    writer.Write((byte)token.Position.y);
					writer.Write((byte)token.Variation);
                }
                result = ms.ToArray();
            }
        }
        return result;
    }
}
