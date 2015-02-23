using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class BoardLayoutImporter : MonoBehaviour 
{
    public BoardLayout BoardLayout;

    void Awake()
    {
        Gameboard.Instance.GameboardReset += Instance_GameStarted;
    }

    private void Instance_GameStarted()
    {
        if (!this.enabled) return;
        ImportBoardLayout(BoardLayout);
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

    public static void ImportBoardLayout(BoardLayout layout)
    {
        WormSpawnerInput wormSpawner = Gameboard.Instance.GetComponent<WormSpawnerInput>();

        Gameboard.Instance.Clear();
        foreach (var token in layout.Tokens)
        {
            if (token.Token.IsWorm)
            {
                if (wormSpawner != null)
                    wormSpawner.CreateWorm(token.Position);
            }
            else
                Gameboard.Instance.AddTileFromToken(token.Token, token.Position, false, true);
        }
        for (int y = 0; y < layout.Rows; y++)
        {
            for (int x = 0; x < layout.Columns; x++)
            {
                Gameboard.Instance.SetBackgroundTileAttribute(x, y, layout.BackgroundTileAttributes[x + y * layout.Columns]);
            }
        }

        Gameboard.Instance.ApplyGravity();
    }
}
