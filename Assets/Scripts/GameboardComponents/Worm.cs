using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameTile))]
public class Worm : MonoBehaviour 
{
	public GameTile WormTile;

    private int _movesTaken = 0;

    public Point MovingTo = Point.zero;
	
    public int MovesTaken { get { return _movesTaken; } }

    void Awake()
    {
        //DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
    }

    private void DebugHUD_MessagesCleared(object sender, System.EventArgs e)
    {
		/*
        if (DisplayMovesTaken)
        {
            DebugHUD.Add("Moves Taken: " + _movesTaken);
        }
		*/
    }

    public bool Move(int x, int y)
    {
        if (Gameboard.Instance.IsValidTileCoordinate(x, y, false))
        {
            GameTile tile = Gameboard.Instance.GetTileAt(x, y);
            if (tile != null)
            {
                if (tile.IsEdible)
                {
                    MovingTo = new Point(x, y);
                    EatToken(tile);
                }
                else if (tile.Pushable)
                {
                    if (tile.Moving) return false;
                    Direction direction = Direction.Right;
                    if (x < WormTile.GridPosition.x) direction = Direction.Left;
                    else if (y > WormTile.GridPosition.y) direction = Direction.Down;
                    else if (y < WormTile.GridPosition.y) direction = Direction.Up;
                    if (!tile.Push(direction))
                    {
                        return false;
                    }
                }
            }
            else
            {
                Gameboard.BackgroundTileAttribute currentTileBackgroundAtt = Gameboard.Instance.GetBackgroundTileAttribute(WormTile.GridPosition.x, WormTile.GridPosition.y);
                Gameboard.BackgroundTileAttribute movingToTileBackgroundAtt = Gameboard.Instance.GetBackgroundTileAttribute(x, y);

                if (currentTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove ||
                    movingToTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove)
                {
                    return false;
                }
            }
        }
        else return false;
        _movesTaken++;
        WormTile.Move(x, y, true);
        Gameboard.Instance.ApplyGravity();
        return true;
    }

    public void EatToken(GameTile tile)
    {
        Gameboard.Instance.DestroyTile(tile, true, false);
    }
}