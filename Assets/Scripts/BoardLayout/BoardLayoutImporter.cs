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

    public static void ImportBoardLayout(BoardLayout layout)
    {
        WormSpawnerInput wormSpawner = Gameboard.Instance.GetComponent<WormSpawnerInput>();

        Gameboard.Instance.Clear();
        if(layout.Columns > 0 && layout.Rows > 0)
        {
            Gameboard.Instance.SetBoardSize(layout.Columns, layout.Rows);
        }

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
}
