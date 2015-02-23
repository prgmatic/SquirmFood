using UnityEngine;
using System.Collections.Generic;

public class BoardLayoutExporter 
{
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

   
}
