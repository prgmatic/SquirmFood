using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class BoardLayoutImporter : MonoBehaviour 
{
    public BoardLayout BoardLayout;

    void Awake()
    {
        Gameboard.Instance.GameStarted += Instance_GameStarted;
    }

    private void Instance_GameStarted()
    {
        if (!this.enabled) return;
        ImportBoardLayout(BoardLayout.Tokens);
    }

    void Start () 
	{
    }
     /*
    private void ImportBoardLayout()
    {
        WormSpawnerInput wormSpawner = GetComponent<WormSpawnerInput>();

        Gameboard.Instance.Clear();
        foreach (var token in BoardLayout.Tokens)
        {
            if(token.Token.IsWorm)
            {
                if(wormSpawner != null)
                    wormSpawner.CreateWorm(token.Position);
            }
            else
                Gameboard.Instance.AddTileFromToken(token.Token, token.Position, false, true);
        }
        Gameboard.Instance.ApplyGravity();
        
    }
    */

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
