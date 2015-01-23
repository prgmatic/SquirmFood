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
        for(int y = gameboard.TilesPerComlumn - 1; y >= 0; y--)
        {
            for(int x = 0; x < gameboard.TilesPerRow; x++)
            {
                int index = Random.Range(0, gameboard.TileSet.Tiles.Count);

                while (MatchThreeRuleSet.CheckForMatch(gameboard.TileSet.Tiles[index].Category, x, y, gameboard))
                {
                    index = Random.Range(0, gameboard.TileSet.Tiles.Count);
                }

                GameTile tile = gameboard.TileSet.CreateTile(index);
                gameboard.AddTile(tile, x, y);
            }
        }
    }

    
}
