using UnityEngine;
using System.Collections;

public class IncreaseWormSize : GameAction 
{
    public int IncreaseBy = 1;

    public override void Execute(Worm worm)
    {
        worm.Length += IncreaseBy;
    }
}
