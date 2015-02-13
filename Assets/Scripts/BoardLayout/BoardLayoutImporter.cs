using UnityEngine;
using System.Collections;

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
        ImportBoardLayout();
    }

    void Start () 
	{
    }

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
}
