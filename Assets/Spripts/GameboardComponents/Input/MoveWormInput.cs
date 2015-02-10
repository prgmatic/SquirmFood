using UnityEngine;
using System.Collections;

public class MoveWormInput : MonoBehaviour 
{
    public KeyCode Up = KeyCode.UpArrow;
    public KeyCode Down = KeyCode.DownArrow;
    public KeyCode Left = KeyCode.LeftArrow;
    public KeyCode Right = KeyCode.RightArrow;


	void Update()
    {
        if (Input.GetKeyDown(Up)) Move(0, -1);
        if (Input.GetKeyDown(Down)) Move(0, 1);
        if (Input.GetKeyDown(Left)) Move(-1, 0);
        if (Input.GetKeyDown(Right)) Move(1, 0);
    }

    public void Move(int x, int y)
    {
        for (int i = 0; i < Gameboard.Instance.gameTiles.Count; i++)
        {
            GameTile tile = Gameboard.Instance.gameTiles[i];
            Worm worm = tile.GetComponent<Worm>();
            if(worm != null)
            {
                worm.Move(worm.Head.GridPosition.x + x, worm.Head.GridPosition.y + y);
            }
        }
    }
}
