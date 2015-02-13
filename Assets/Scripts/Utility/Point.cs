using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Point {
    public int x;
    public int y;

    public static Point zero { get { return new Point(0, 0); } }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
            return false;

        // Return true if the fields match:
        return obj is Point && this == (Point)obj;
    }
    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }
    public bool Equals(Point p)
    {
        // If parameter is null return false:
        if ((object)p == null)
            return false;

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }
    public static bool operator ==(Point p1, Point p2)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(p1, p2))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)p1 == null) || ((object)p2 == null))
        {
            return false;
        }
        return p1.x == p2.x && p1.y == p2.y;
    }
    public static bool operator !=(Point p1, Point p2)
    {
        return !(p1 == p2);
    }
    public override string ToString()
    {
        return "X: " + x + "Y: " + y;
    }
}
