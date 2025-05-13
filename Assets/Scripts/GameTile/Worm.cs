using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameTile))]
public class Worm : MonoBehaviour
{
    public bool CanFreeMove = false;
    public Direction FacingDirection = Direction.Right;

    private GameTile _gameTile;
    private Animator _animator;
    private GameObject _wormSprite;
    private ParticleSystem _emitter;

    private int _movesTaken = 0;


    public int MovesTaken
    {
        get { return _movesTaken; }
    }

    void Awake()
    {
        _gameTile = GetComponent<GameTile>();
        _animator = GetComponent<Animator>();
        _emitter = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        _animator.SetFloat("FacingDirection", (int)FacingDirection);

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


    public bool Move(Direction direction)
    {
        Point targetPos = direction switch
        {
            Direction.Up => new Point(0, -1),
            Direction.Down => new Point(0, 1),
            Direction.Left => new Point(-1, 0),
            Direction.Right => new Point(1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
        targetPos.x += _gameTile.GridPosition.x;
        targetPos.y += _gameTile.GridPosition.y;
        return Move(targetPos.x, targetPos.y);
    }

    public bool Move(int x, int y)
    {
        var canMove = CanMoveTo(x, y);
        FacingDirection = Utils.GetDirection(_gameTile.GridPosition, new Point(x, y));
        _animator.SetInteger("AnimationType", (int)WormAnimationType.Move);
        _animator.SetInteger("MoveDirection", (int)FacingDirection);

        if (canMove)
        {
            GameTile tileAtDestination = Gameboard.Instance.GetTileAt(x, y);
            if (tileAtDestination != null)
            {
                if (tileAtDestination.IsEdible)
                {
                    EatToken(tileAtDestination);
                    _animator.SetInteger("AnimationType", (int)WormAnimationType.Eat);
                }
                else if (tileAtDestination.Pushable)
                {
                    tileAtDestination.Push(FacingDirection);
                    _animator.SetInteger("AnimationType", (int)WormAnimationType.Push);
                }
            }

            _gameTile.Move(x, y);
        }
        else
        {
            _animator.SetInteger("AnimationType", (int)WormAnimationType.Invalid);
        }

        Gameboard.Instance.ApplyGravity();
        _animator.SetTrigger("Move");
        return canMove;
    }

    public bool CanMoveTo(int x, int y)
    {
        // If destination is not a valid coordinate, worm can't move
        if (!Gameboard.Instance.IsValidTileCoordinate(x, y, false))
            return false;

        GameTile tileAtDestination = Gameboard.Instance.GetTileAt(x, y);
        // Is there a tile at destination
        if (tileAtDestination != null)
        {
            // If tile is edible or pushable, the worm can move
            var dir = Utils.GetDirection(_gameTile.GridPosition, new Point(x, y));
            return tileAtDestination.IsEdible || tileAtDestination.CanPush(dir);
        }
        // No tile at destination

        if (CanFreeMove)
            return true;
        // If mud is at destination, worm can move
        else
            return Gameboard.Instance.GetBackgroundTileAttribute(x, y) ==
                   Gameboard.BackgroundTileAttribute.FreeMove;
    }

    public void EatToken(GameTile tile)
    {
        Gameboard.Instance.DestroyTile(tile, true, false);
    }

    public void EmitParticles(int numberOfParticles)
    {
        _emitter.Emit(numberOfParticles);
    }
}