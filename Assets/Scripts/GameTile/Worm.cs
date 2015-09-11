using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameTile))]
public class Worm : MonoBehaviour
{
    public bool CanFreeMove = false;

    private GameTile _gameTile;
    private Animator _animator;
    private GameObject _wormSprite;

    private int _movesTaken = 0;


    public int MovesTaken { get { return _movesTaken; } }

    void Awake()
    {
        _gameTile = GetComponent<GameTile>();
        _animator = GetComponent<Animator>();
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
        _animator.SetInteger("AnimationType", (int)WormAnimationType.Move);

        if (Gameboard.Instance.IsValidTileCoordinate(x, y, false))
        {
            GameTile tile = Gameboard.Instance.GetTileAt(x, y);
            if (tile != null)
            {
                if (tile.IsEdible)
                {
                    _animator.SetInteger("AnimationType", (int)WormAnimationType.Eat);
                    EatToken(tile);
                }
                else if (tile.Pushable)
                {
                    if (tile.Moving) return false;
                    Direction direction = Direction.Right;
                    if (x < _gameTile.GridPosition.x) direction = Direction.Left;
                    else if (y > _gameTile.GridPosition.y) direction = Direction.Down;
                    else if (y < _gameTile.GridPosition.y) direction = Direction.Up;
                    if (tile.Push(direction))
                    {
                        _animator.SetInteger("AnimationType", (int)WormAnimationType.Push);
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                Gameboard.BackgroundTileAttribute currentTileBackgroundAtt = Gameboard.Instance.GetBackgroundTileAttribute(_gameTile.GridPosition.x, _gameTile.GridPosition.y);
                Gameboard.BackgroundTileAttribute movingToTileBackgroundAtt = Gameboard.Instance.GetBackgroundTileAttribute(x, y);

                if (!CanFreeMove)
                {
                    if (currentTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove ||
                        movingToTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove)
                    {
                        return false;
                    }
                }
            }
        }
        else return false;
        _movesTaken++;
        var d = Utils.GetDirection(_gameTile.GridPosition, new Point(x, y));
        //MudSmearController.Instance.AddSmear(WormTile.GridPosition, d);

        var dir = Utils.GetDirection(_gameTile.GridPosition, new Point(x, y));
        _animator.SetInteger("MoveDirection", (int)dir);
        _animator.SetTrigger("Move");
        

        _gameTile.Move(x, y, true);
        Gameboard.Instance.ApplyGravity();
        return true;
    }

    public void EatToken(GameTile tile)
    {
        Gameboard.Instance.DestroyTile(tile, true, false);
    }
}