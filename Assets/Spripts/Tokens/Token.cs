using UnityEngine;
using System.Collections;

public class Token : ScriptableObject 
{
    public float Fuel = 0f;
    public bool IsNuisance = false;
    public bool IsEdible = true;
    public bool CanFall = true;
    public bool IsWorm = false;
    public int Width = 1;
    public int Height = 1;

    public Rectangle GetBounds(int x, int y)
    {
        return new Rectangle(x, y, Width, Height);
    }
}
