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


    public int MovesTaken { get { return _movesTaken; } }

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



    public bool Move(int x, int y)
    {
        FacingDirection = Utils.GetDirection(_gameTile.GridPosition, new Point(x, y));
        _animator.SetInteger("AnimationType", (int)WormAnimationType.Move);
        _animator.SetInteger("MoveDirection", (int)FacingDirection);

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
                    if (tile.Push(FacingDirection))
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
                    if (movingToTileBackgroundAtt == Gameboard.BackgroundTileAttribute.LimitedMove)
                    {
                        return false;
                    }
                }
            }
        }
        else return false;
        _movesTaken++;
        //MudSmearController.Instance.AddSmear(WormTile.GridPosition, d);

        _animator.SetTrigger("Move");


        _gameTile.Move(x, y, true);
        Gameboard.Instance.ApplyGravity();
        return true;
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