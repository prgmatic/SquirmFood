using UnityEngine;
//using UnityEditor;
//using System.IO;
using System.Collections;

public static class Utils
{
    private static Sprite _dummySprite;
    
    public static Sprite DummySprite
    {
        get
        {
            if(_dummySprite == null)
            {
                Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                _dummySprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(.5f, .5f), 1);
            }
            return _dummySprite;
        }
    }


    public static Point CursorGridPosotion
	{
		get
		{
			return Gameboard.Instance.WorldPositionToGridPosition(CursorPositionInWorld);
		}
	}
	public static Plane WorldPlane = new Plane(Vector3.forward, 0f);
	public static Vector3 CursorPositionInWorld
	{
		get
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float rayDistance;
			if (WorldPlane.Raycast(ray, out rayDistance))
			{
				return ray.GetPoint(rayDistance);
			}
			return Vector3.zero;
		}
	}

    public static float GetWidth(this Sprite sprite)
    {
        return sprite.bounds.size.x;
    }
    public static float GetHeight(this Sprite sprite)
    {
        return sprite.bounds.size.y;
    }

    public static float Vector2Angle(Vector2 from, Vector2 to)
    {
        float deltaY = to.y - from.y;
        float deltaX = to.x - from.x;
        return Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
    }

    public static Quaternion QuaternionLookAt(Vector2 from, Vector2 to)
    {
        float angle = Vector2Angle(from, to);
        return Quaternion.Euler(0, 0, angle);
    }

	public static Direction GetDirection(Point startPos, Point endPos)
	{
		if (startPos.x > endPos.x) return Direction.Left;
		if (startPos.y < endPos.y) return Direction.Down;
		if (startPos.y > endPos.y) return Direction.Up;
		return Direction.Right;
	}

	#region Vector Utils
	public static Vector3 SetX(this Vector3 v3, float x)
	{
		return new Vector3(x, v3.y, v3.z);
	}
	public static Vector3 SetY(this Vector3 v3, float y)
	{
		return new Vector3(v3.x, y, v3.z);
	}
	public static Vector3 SetZ(this Vector3 v3, float z)
	{
		return new Vector3(v3.x, v3.y, z);
	}

	public static Vector2 SetX(this Vector2 v2, float x)
	{
		return new Vector2(x, v2.y);
	}
	public static Vector2 SetY(this Vector2 v2, float y)
	{
		return new Vector2(v2.x, y);
	}
	#endregion
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
