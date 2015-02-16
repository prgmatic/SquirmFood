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
        return layout;
    }

   
}
