using UnityEngine;
using System.Collections;

[System.Serializable]
public class Resource 
{

    public ResourceType Type;
    public int Value;

	public Resource(ResourceType type, int value = 0)
    {
        this.Type = type;
        this.Value = value;
    }

    public enum ResourceType
    {
        Fuel,
        Tissue
    }
}
