using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatCompare 
{
    public float Value;
    public ComparatorType Comparator;
    public string Name;

    public FloatCompare(string name, int value = 0)
    {
        this.Name = name;
        this.Value = value;
    }

    public bool Compare(float num)
    {
        switch(Comparator)
        {
            case ComparatorType.Equal:
                return num == Value;
            case ComparatorType.GreaterThan:
                return num > Value;
            case ComparatorType.LessThan:
                return num < Value;
            case ComparatorType.GreaterThanOrEqual:
                return num >= Value;
            case ComparatorType.LessThanOrEqual:
                return num <= Value;
        }
        return false;
    }

    public enum ComparatorType
    {
        LessThan,
        GreaterThan,
        Equal,
        LessThanOrEqual,
        GreaterThanOrEqual
    }
}
