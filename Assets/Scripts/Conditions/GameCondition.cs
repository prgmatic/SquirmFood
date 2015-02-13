using UnityEngine;
using System.Collections;

public class GameCondition : ScriptableObject
{
	public virtual bool ConditionMet()
    {
        return false;
    }
}
