using UnityEngine;
using System.Collections.Generic;

public class PipeObject : MonoBehaviour
{
	private NeighborInfo _ni;

	public int X = 0;
	public int Y = 0;

	private static GameObject _iPipe;
	private static GameObject _lPipe;
	private static GameObject _tPipe;
	private static GameObject _xPipe;

	void Awake()
	{
		_ni = new NeighborInfo();
		_iPipe = Resources.Load<GameObject>("IPipe");
		_lPipe = Resources.Load<GameObject>("LPipe");
		_tPipe = Resources.Load<GameObject>("TPipe");
		_xPipe = Resources.Load<GameObject>("XPipe");
	}

	public void UpdatePipe(bool self, bool top, bool bottom, bool left, bool right)
	{
		_ni.Top = top;
		_ni.Bottom = bottom;
		_ni.Left = left;
		_ni.Right = right;

		if (!self) DestroyChildren();
		else SetPipe();
	}

	private void SetPipe()
	{
		DestroyChildren();
		float rotation;
		GameObject pipe = Instantiate<GameObject>(SelectPrefab(out rotation, _ni));
		pipe.transform.SetParent(this.transform);
		pipe.transform.localPosition = Vector3.zero;
		pipe.transform.localRotation = Quaternion.Euler(0, 0, rotation);
		pipe.layer = this.gameObject.layer;
		pipe.isStatic = true;
	}

	public static string GetPipeNameAndRotation(NewBoardLayout layout, int x, int y, out float rotation)
	{
		rotation = 0f;
		if (!layout.IsValidCoordinate(x, y)) return string.Empty;
		NeighborInfo ni = GetNeightborInfo(layout, x, y);
		return SelectPrefab(out rotation, ni).name;
	}

	private static NeighborInfo GetNeightborInfo(NewBoardLayout layout, int x, int y)
	{
		bool top = IsWall(layout, x, y - 1);
		bool bottom = IsWall(layout, x, y + 1);
		bool left = IsWall(layout, x - 1, y);
		bool right = IsWall(layout, x + 1, y);

		return new NeighborInfo(top, bottom, left, right);
	}

	private static bool IsWall(NewBoardLayout layout, int x, int y)
	{
		if (!layout.IsValidCoordinate(x, y)) return false;
		var tile = layout.GetGameTileAt(x, y);
		if(tile != null)
		{
			return !tile.Pushable && !tile.IsEdible && !tile.CanFall && !tile.IsWorm;
        }
		return false;
	}

	private static GameObject SelectPrefab(out float rotation, NeighborInfo ni)
	{
		rotation = 0f;
		GameObject result = _xPipe;
		var numberOfNeighbors = ni.NumberOfNeighbors;
		if (ShouldUseIPipe(out rotation, ni))
			result = _iPipe;
		else if (ShouldUseLPipe(out rotation, ni))
			result = _lPipe;
		else if (ShouldUseTPipe(out rotation, ni))
			result = _tPipe;
		return result;
	}

	private void DestroyChildren()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	private static bool ShouldUseIPipe(out float rotation, NeighborInfo ni)
	{
		rotation = 0f;
		if (ni.Top && ni.Bottom && !ni.Left && !ni.Right)
		{
			rotation = 0f;
			return true;
		}
	    if (!ni.Top && !ni.Bottom && ni.Left && ni.Right)
		{
			rotation = 90f;
			return true;
		}
		var n = ni.NumberOfNeighbors;
		if(n == 1)
		{
			if (ni.Left || ni.Right) rotation = 90f;
			return true;
		}
		return false;
	}

	private static bool ShouldUseLPipe(out float rotation, NeighborInfo ni)
	{
		if (ni.Top && !ni.Bottom && ni.Left && !ni.Right) // ni.Top ni.Left
		{
			rotation = 0f;
			return true;
		}
		if (ni.Top && !ni.Bottom && !ni.Left && ni.Right) // ni.Top ni.Right
		{
			rotation = 90f;
			return true;
		}
		if (!ni.Top && ni.Bottom && !ni.Left && ni.Right) // ni.Bottom ni.Right
		{
			rotation = 180f;
			return true;
		}
		if (!ni.Top && ni.Bottom && ni.Left && !ni.Right) // ni.Bottom ni.Left
		{
			rotation = 270f;
			return true;
		}
		rotation = 0f;
		return false;
	}

	private static bool ShouldUseTPipe(out float rotation, NeighborInfo ni)
	{
		rotation = 0f;
		var n = ni.NumberOfNeighbors;
		if (n == 3)
		{
			if (!ni.Right) rotation = 0f;
			if (!ni.Bottom) rotation = 90f;
			if (!ni.Left) rotation = 180f;
			if (!ni.Top) rotation = 270f;
			return true;
		}
		return false;
	}
}

public class NeighborInfo
{
	public bool Top = false;
	public bool Bottom = false;
	public bool Left = false;
	public bool Right = false;

	public int NumberOfNeighbors { get { return GetNumberOfNeighbors(); } }

	public NeighborInfo()
	{

	}

	public NeighborInfo(bool top, bool bottom, bool left, bool right)
	{
		this.Top = top;
		this.Bottom = bottom;
		this.Left = left;
		this.Right = right;
	}

	private int GetNumberOfNeighbors()
	{
		int result = 0;
		if (Top) result++;
		if (Bottom) result++;
		if (Left) result++;
		if (Right) result++;
		return result;
	}
}
