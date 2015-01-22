using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeBoardInitializer : MonoBehaviour
{
    Gameboard gameboard;

    void Awake()
    {
        gameboard = GetComponent<Gameboard>();
        FillGameBoard();
    }

    void FillGameBoard()
    {
        for(int y = 0; y < gameboard.TilesPerComlumn; y++)
        {
            for(int x = 0; x < gameboard.TilesPerRow; x++)
            {
                GameTile tile = gameboard.TileSet.CreateTile(Random.Range(0, gameboard.TileSet.Tiles.Count));
                gameboard.AddTile(tile, x, y);
            }
        }
    }
	
}
