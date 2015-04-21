using UnityEngine;
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

    
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
