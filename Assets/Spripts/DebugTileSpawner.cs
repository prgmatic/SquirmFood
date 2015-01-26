using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class DebugTileSpawner : MonoBehaviour
{
    private Gameboard gameboard;
    private string[] numberStrings;

    void Awake()
    {
        gameboard = this.GetComponent<Gameboard>();
        numberStrings = new string[10];
        for(int i = 0; i < 10; i++)
        {
            numberStrings[i] = i.ToString();
        }
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Point point = gameboard.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            gameboard.DestroyTileAt(point.x, point.y);
        }
        for(int i = 0; i < 10; i++)
        {
            if(Input.GetKeyDown(numberStrings[i]))
            {
                int index = i - 1;
                if (i == 0) index = 9;

                if(index < gameboard.Tiles.Count)
                {
                    Point point = gameboard.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    gameboard.AddTile(index, point.x, point.y);
                }
            }
        }
    }
}
