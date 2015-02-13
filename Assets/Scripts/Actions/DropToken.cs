using UnityEngine;
using System;
using System.Collections;

public class DropToken : GameAction 
{
    public enum LocationToDropToken
    {
        WormTail,
        WormHead,
        MovingTo
    }

    public LocationToDropToken Where;
    public Token TokenToDrop;

    public override void Execute(Worm worm)
    {
        Point pos = Point.zero;
        switch(Where)
        {
            case LocationToDropToken.WormTail:
                pos = worm.Tail.GridPosition;
                break;
            case LocationToDropToken.WormHead:
                pos = worm.Head.GridPosition;
                break;
            case LocationToDropToken.MovingTo:
                pos = worm.MovingTo;
                break;
        }
        Gameboard.Instance.AddTileFromToken(TokenToDrop, pos, true, true);
    }
}
