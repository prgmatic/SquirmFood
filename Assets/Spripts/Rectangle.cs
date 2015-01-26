using UnityEngine;
using System.Collections;

public class Rectangle
{
    public int x;
    public int y;
    public int width;
    public int height;

    public int Left { get { return x; } }
    public int Right { get { return x + width; } }
    public int Top { get { return y; } }
    public int Bottom { get { return y + height; } }

    public Rectangle()
    {
        this.x = 0;
        this.y = 0;
        this.width = 0;
        this.height = 0;
    }

    public Rectangle(Rectangle source)
    {
        this.x = source.x;
        this.y = source.y;
        this.width = source.width;
        this.height = source.height;
    }

    public Rectangle(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
	
    public bool Intersects(Rectangle rectB)
    {
        Rectangle rectA = this;
        return !(rectA.Left >= rectB.Right || rectA.Right <= rectB.Left ||
                 rectA.Top >= rectB.Bottom || rectA.Bottom <= rectB.Top);
    }

    public bool Contains(Rectangle rectB)
    {
        return Left <= rectB.Left && Right >= rectB.Right &&
               Top <= rectB.Top && Bottom >= Bottom;
    }
}
