using UnityEngine;
using System.Collections.Generic;

public class BoardInitalizer : MonoBehaviour 
{
    public List<Token> tokensToSpawn;

    void Start()
    {
        FillGameboard();
    }

    private void FillGameboard()
    {
        for(int y = 0; y < Gameboard.Instance.Rows; y++)
        {
            for(int x = 0; x< Gameboard.Instance.Columns; x++)
            {
                int index = Random.Range(0, tokensToSpawn.Count);
                Gameboard.Instance.AddTileFromToken(tokensToSpawn[index], x, y, false);
            }
        }
    }
}
