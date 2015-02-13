using UnityEngine;
using System.Collections;

public class ResourceCondition : GameCondition 
{
    public Resource.ResourceType ResourceType;
    public FloatCompare Comparator = new FloatCompare("Amount");

    public override bool ConditionMet()
    {
        return Comparator.Compare(ResourcePool.Instance.GetResource(ResourceType).Value);
    }
}
