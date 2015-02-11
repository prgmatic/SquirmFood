using UnityEngine;
using System.Collections;

public class BoardLayoutImporter : MonoBehaviour 
{
    public BoardLayout BoardLayout;
	void Start () 
	{
        Gameboard.Instance.Clear();

        foreach(var token in BoardLayout.Tokens)
        {
            Gameboard.Instance.AddTileFromToken(token.Token, token.Position, false, true);
        }
        Gameboard.Instance.ApplyGravity();
    }
}
