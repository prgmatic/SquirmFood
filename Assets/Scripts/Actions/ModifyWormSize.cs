using UnityEngine;
using System.Collections;

public class ModifyWormSize : GameAction 
{
    public int ModifyBy = 1;

    public override void Execute(Worm worm)
    {
        worm.Length += ModifyBy;
    }
}
