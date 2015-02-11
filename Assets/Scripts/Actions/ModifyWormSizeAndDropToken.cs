using UnityEngine;
using System.Collections;

public class ModifyWormSizeAndDropToken : GameAction 
{
    public int ModifyBy = 1;
    public DropToken.LocationToDropToken Where;
    public Token TokenToDrop;

    public override void Execute(Worm worm)
    {
        worm.Length += ModifyBy;

        Point pos = Point.zero;
        switch (Where)
        {
            case DropToken.LocationToDropToken.WormTail:
                pos = worm.Tail.GridPosition;
                break;
            case DropToken.LocationToDropToken.WormHead:
                pos = worm.Head.GridPosition;
                break;
        }
        Gameboard.Instance.AddTileFromToken(TokenToDrop, pos, true, true);
    }
}
