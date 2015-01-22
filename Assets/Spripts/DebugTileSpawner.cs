using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class DebugTileSpawner : MonoBehaviour
{
    private Gameboard gameboard;

    void Awake()
    {
        gameboard = this.GetComponent<Gameboard>();
    }

    void Update()
    {
        for(int i = 0; i < 10; i++)
        {
            if(Input.GetKeyDown(i.ToString()))
            {
                int index = i - 1;
                if (i == 0) index = 9;

                if(index < gameboard.TileSet.Tiles.Count)
                {
                    int x;
                    int y;

                    if(gameboard.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y))
                    {
                        gameboard.TileSet.CreateTile(index, x, y);
                    }
                }
            }
        }
    }
	
}
