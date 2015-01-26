using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeBoardInitializer : MonoBehaviour
{
    Gameboard gameboard;

    void Start()
    {
        gameboard = GetComponent<Gameboard>();
        FillGameBoard();
    }
    void FillGameBoard()
    {
        for(int y = gameboard.Rows - 1; y >= 0; y--)
        {
            for(int x = 0; x < gameboard.Columns; x++)
            {
                int index = Random.Range(0, gameboard.Tiles.Count);

                while (MatchThreeRuleSet.IsMatchAt(gameboard.Tiles[index].Category, x, y, gameboard))
                {
                    index = Random.Range(0, gameboard.Tiles.Count);
                }
                gameboard.AddTile(index, x, y);
            }
        }
    }
}
