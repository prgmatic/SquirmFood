using UnityEngine;
using System.Collections.Generic;

public class MultiCondition : GameCondition 
{
    public List<GameCondition> Conditions = new List<GameCondition>();

    public override bool ConditionMet()
    {
        if (Conditions.Count == 0) return false;
        foreach(var condition in Conditions)
            if (!condition.ConditionMet()) return false;
        return true;
    }
}
