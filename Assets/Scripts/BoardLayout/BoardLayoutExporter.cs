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
