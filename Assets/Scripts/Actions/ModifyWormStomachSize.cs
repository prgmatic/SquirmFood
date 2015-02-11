using UnityEngine;
using System.Collections;

public class ModifyWormStomachSize : GameAction 
{
    public int ModifyBy = 1;

    public override void Execute(Worm worm)
    {
        worm.StomachSize += ModifyBy;
    }
}
