using UnityEngine;
using System.Collections.Generic;

public class MultiAction : GameAction 
{
    public List<GameAction> Actions = new List<GameAction>();

    public override void Execute(Worm worm)
    {
        foreach(var action in Actions)
        {
            action.Execute(worm);
        }
    }
}
