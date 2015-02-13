using UnityEngine;
using System.Collections;

public class MovesTakenCondition : GameCondition 
{
    public FloatCompare Moves = new FloatCompare("Moves");

    public override bool ConditionMet()
    {
        foreach(var tile in Gameboard.Instance.gameTiles)
        {
            Worm worm = tile.GetComponent<Worm>();
            if(worm != null)
            {
                return Moves.Compare(worm.MovesTaken);
            }
        }
        return false;
    }
}
