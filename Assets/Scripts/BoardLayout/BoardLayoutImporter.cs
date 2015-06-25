using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class BoardLayoutImporter
{

    public static void ImportBoardLayout(BoardLayout layout)
    {
        Gameboard.Instance.Clear();
        if(layout.Columns > 0 && layout.Rows > 0)
        {
            Gameboard.Instance.SetBoardSize(layout.Columns, layout.Rows);
        }

        foreach (var token in layout.Tokens)
        {
			if (token.Token.IsWorm)
			{
				WormCreator.CreateWorm(token.Position);
			}
			else
			{
				var tile = Gameboard.Instance.AddTileFromToken(token.Token, token.Position, false, true);
				tile.Variation = token.Variation;
			}
        }
        if (layout.BackgroundTileAttributes.Length == layout.Columns * layout.Rows)
        {
            for (int y = 0; y < layout.Rows; y++)
            {
                for (int x = 0; x < layout.Columns; x++)
                {
                    Gameboard.Instance.SetBackgroundTileAttribute(x, y, layout.BackgroundTileAttributes[x + y * layout.Columns]);
                }
            }
        }
        Gameboard.Instance.ApplyGravity();
    }

    public static BoardLayout GetBoardLayoutFromBinary(byte[] data)
    {
        BoardLayout result = ScriptableObject.CreateInstance<BoardLayout>();

        var tokens = Resources.FindObjectsOfTypeAll<Token>()
            .Where(t => t.ID != 0 && t.ID != 255)
            .Select(t => t)
            .ToArray();

        using (MemoryStream ms = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
#pragma warning disable 0219
                int exportVersion = reader.ReadInt32();
#pragma warning restore 0219
                result.Columns = reader.ReadInt32();
                result.Rows = reader.ReadInt32();

                result.BackgroundTileAttributes = new Gameboard.BackgroundTileAttribute[result.Columns * result.Rows];
                for (int y = 0; y < result.Rows; y++)
                {
                    for (int x = 0; x < result.Columns; x++)
                    {
                        if (reader.ReadBoolean())
                            result.BackgroundTileAttributes[x + y * result.Columns] = Gameboard.BackgroundTileAttribute.FreeMove;
                        else
                            result.BackgroundTileAttributes[x + y * result.Columns] = Gameboard.BackgroundTileAttribute.LimitedMove;
                    }
                }

                int numberOfTokens = reader.ReadInt32();
                for (int i = 0; i < numberOfTokens; i++)
                {
                    //result.Tokens.Add(new BoardLayout.TokenAtPoint())
                    int id = reader.ReadByte();
                    int x = reader.ReadByte();
                    int y = reader.ReadByte();
					int variation = 0;
					if (exportVersion > 0) variation = reader.ReadByte();

                    if (id == 0 || id == 255)
                        continue;

                    foreach (var token in tokens)
                    {
                        if (token.ID == id)
                        {
                            result.Tokens.Add(new BoardLayout.TokenAtPoint(token, new Point(x, y), variation));
                        }
                    }
                }
            }
        }
        return result;
    }

    public static void ImportBinary(byte[] data)
    {
        ImportBoardLayout(GetBoardLayoutFromBinary(data));
    }
}
