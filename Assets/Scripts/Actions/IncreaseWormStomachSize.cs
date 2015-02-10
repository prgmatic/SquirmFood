using UnityEngine;
using System.Collections;

public class IncreaseWormStomachSize : GameAction 
{
    public int IncreaseBy = 1;

    public override void Execute(Worm worm)
    {
        worm.StomachSize += IncreaseBy;
    }
}
