using UnityEngine;
using System.Collections.Generic;

public class Token : ScriptableObject 
{
    public List<Resource> Resources = new List<Resource>();
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
