using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class DebugTileSpawner : MonoBehaviour
{
    private string[] numberStrings;

    public List<Token> TokensToSpawn;

    void Awake()
    {
        numberStrings = new string[10];
        for(int i = 0; i < 10; i++)
        {
            numberStrings[i] = i.ToString();
        }
    }
    void Update()
    {
        if (!Gameboard.Instance.AcceptingInput) return;
        if(Input.GetMouseButtonDown(1))
        {
            Point point = Gameboard.Instance.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Gameboard.Instance.DestroyTileAt(point.x, point.y);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Point point = Utils.CursorGridPosotion;
            if(Gameboard.Instance.IsValidTileCoordinate(point))
            {
                Gameboard.BackgroundTileAttribute ta = Gameboard.Instance.GetBackgroundTileAttribute(point.x, point.y);
                if (ta == Gameboard.BackgroundTileAttribute.FreeMove)
                    ta = Gameboard.BackgroundTileAttribute.LimitedMove;
                else ta = Gameboard.BackgroundTileAttribute.FreeMove;
                Gameboard.Instance.SetBackgroundTileAttribute(point.x, point.y, ta);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(numberStrings[i]))
            {
                int index = i - 1;
                if (i == 0) index = 9;

                if (index < TokensToSpawn.Count)
                {
                    Token token = TokensToSpawn[index];
                    Point point = Gameboard.Instance.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    Gameboard.Instance.DestroyTilesInBounds(token.GetBounds(point.x, point.y), false, false);
                    Gameboard.Instance.AddTileFromToken(token, point.x, point.y);
                }
            }
        }
    }
}
