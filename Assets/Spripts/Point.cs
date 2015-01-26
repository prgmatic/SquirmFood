using UnityEngine;
using System.Collections;

public struct Point {
    public int x;
    public int y;

    public static Point zero { get { return new Point(0, 0); } }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "X: " + x + "Y: " + y;
    }
}
