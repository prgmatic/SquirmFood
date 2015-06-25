using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameTile))]
public class Worm : MonoBehaviour
{
    private GameTile _wormTile;
    public GameTile WormTile
    {
        get { return _wormTile; }
        set
        {
            _wormTile = value;
            _animator = value.GetComponent<Animator>();
        }
    }
    private Animator _animator;
    private GameObject _wormSprite;

    private int _movesTaken = 0;

    public Point MovingTo = Point.zero;

    public int MovesTaken { get { return _movesTaken; } }

    void Awake()
    {
        Debug.Log("entered game");
        //_wormSprite = (GameObject)Instantiate(Resources.Load("Worm"));
        //_animator = _wormSprite.GetComponent<Animator>();
        //worm.transform.SetParent(WormTile.transform);
        //worm.transform.localPosition = Vector3.zero;
        //DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
    }

    void Update()
    {
        //_wormSprite.transform.position = WormTile.transform.position;
        //WormTile.Hide();
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
        bool eating = false;

        if (Gameboard.Instance.IsValidTileCoordinate(x, y, false))
        {
            GameTile tile = Gameboard.Instance.GetTileAt(x, y);
            if (tile != null)
            {
                if (tile.IsEdible)
                {
                    MovingTo = new Point(x, y);
                    EatToken(tile);
                    eating = true;
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
                else return false;
            }
            else
            {
                Gameboard.BackgroundTileAttribute currentTileBackgroundAtt = Gameboard.Instance.GetBackgroundTileAttribute(WormTile.GridPosition.x, WormTile.GridPosition.y);
                Gameboard.BackgroundTileAttribute movingToTileBackgroundAtt = Gameboard.Instance.GetBackgroundTileAttribute(x, y);

                if (currentTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove ||
                    movingToTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove)
                {
                    //return false;
                }
            }
        }
        else return false;
        _movesTaken++;
        var d = Utils.GetDirection(WormTile.GridPosition, new Point(x, y));
        //MudSmearController.Instance.AddSmear(WormTile.GridPosition, d);

        var dir = Utils.GetDirection(WormTile.GridPosition, new Point(x, y));
        _animator.SetBool("Eat", eating);
        _animator.SetTrigger(1);
        switch(dir)
        {
            case Direction.Up:
                _animator.SetTrigger("MoveUp");
                break;
            case Direction.Down:
                _animator.SetTrigger("MoveDown");
                break;
            case Direction.Left:
                _animator.SetTrigger("MoveLeft");
                break;
            case Direction.Right:
                _animator.SetTrigger("MoveRight");
                break;
        }

        WormTile.Move(x, y, true);
        Gameboard.Instance.ApplyGravity();
        return true;
    }

    public void EatToken(GameTile tile)
    {
        Gameboard.Instance.DestroyTile(tile, true, false);
    }
}