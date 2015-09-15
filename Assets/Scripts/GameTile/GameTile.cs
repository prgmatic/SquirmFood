using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class GameTile : MonoBehaviour
{
	#region EventsHandlers
	public delegate void GameTileEvent(GameTile sender);
	public delegate void GameTileGridMovedEvent(GameTile sender, Rectangle oldGridBounds);
	public event GameTileEvent SettledFromFall;
	public event GameTileEvent SettledFromMove;
	public event GameTileGridMovedEvent GridPositionMoved;
	#endregion

	#region Variables
	public static float MoveTime = .2f;

	public float Acceleration = 3f;
	[HideInInspector]
	public string Category = "";

	private SpriteRenderer _renderer;
	private Vector3 _velocity = Vector3.zero;
	private bool _moving = false;
	//private Point _size = new Point(1, 1);
	private Point _gridPosition = Point.zero;
	private Rectangle gravityBounds;
	private bool _flipped = false;
	private int _variation = 0;
	#endregion

	#region Properties
	public Vector3 WorldPosition { get { return this.transform.position; } set { this.transform.position = value; } }

    public int ID = 0;
    public int Width = 1;
    public int Height = 1;
	public Point Size { get { return new Point(Width, Height); } set { Width = value.x; Height = value.y; } }

	public int GridLeft { get { return _gridPosition.x; } }
	public int GridRight { get { return _gridPosition.x + Width; } }
	public int GridTop { get { return _gridPosition.y; } }
	public int GridBottom { get { return _gridPosition.y + Height; } }
	public Point GridPosition { get { return _gridPosition; } set { _gridPosition = value; } }

	public float WorldLeft
	{
		get { return WorldPosition.x - 0.5f * Width; }
		set { WorldPosition = new Vector3(value + 0.5f * Width, WorldPosition.y, WorldPosition.z); }
	}
	public float WorldRight { get { return WorldPosition.x + 0.5f * Width; } }
	public float WorldTop
	{
		get { return WorldPosition.y + 0.5f * Height; }
		set { WorldPosition = new Vector3(WorldPosition.x, value - 0.5f * Height, WorldPosition.z); }
	}
	public float WorldBottom { get { return WorldPosition.y - 0.5f * Height; } }

	public Rectangle GridBounds { get { return new Rectangle(GridLeft, GridTop, Width, Height); } }

	public bool Moving { get { return _moving; } }
	public Sprite Sprite { get { return _renderer.sprite; } set { _renderer.sprite = value; } }
	public Color Color { get { return _renderer.color; } set { _renderer.color = value; } }


	public bool CanFall = false;
	public bool IsEdible = false;
	public bool IsWorm = false;
    public bool Pushable = false;
	public bool IsWall { get { return !Pushable && !IsEdible && !CanFall && !IsWorm; } }
	public int Variation
	{
		get { return _variation; }
		set
		{
			_variation = value;
		}
	}
	#endregion

	void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}
	void Update()
	{
         
	}
	public void ApplyGravity()
	{
		if (!CanFall || _moving) return;
		if (!Gameboard.Instance.GridBounds.Contains(this.GridBounds)) return;
		if (GridBottom < Gameboard.Instance.Rows)
		{
			gravityBounds = new Rectangle(GridLeft, GridTop + 1, Width, Height);
			while (gravityBounds.Bottom <= Gameboard.Instance.Rows && Gameboard.Instance.NumberOfTilesInBounds(gravityBounds, this) == 0)
			{
				gravityBounds.y++;
			}

			if (gravityBounds.y - 1 > GridTop)
			{
				Rectangle oldGridBounds = GridBounds;
				_gridPosition.y = gravityBounds.y - 1;
				if (GridPositionMoved != null)
					GridPositionMoved(this, oldGridBounds);
				StopCoroutine("FallToTarget");
				try
				{
					StartCoroutine("FallToTarget");
				}
				catch
				{
					Debug.Log("oh no");
				}
			}
			if (GridBottom == Gameboard.Instance.Rows && (Width > 1 || Height > 1))
			{
				Rectangle oldGridBounds = GridBounds;
				_gridPosition.y = Gameboard.Instance.Rows + 10;
				if (GridPositionMoved != null)
					GridPositionMoved(this, oldGridBounds);
				StopCoroutine("FallToTarget");
				StartCoroutine("FallToTarget");
			}
		}
	}
	public void Move(int x, int y, bool animate = true)
	{
		Rectangle oldGridBounds = GridBounds;
		_gridPosition.x = x;
		_gridPosition.y = y;
		if (GridPositionMoved != null)
			GridPositionMoved(this, oldGridBounds);
		if (animate)
		{
			StopCoroutine("TransitionToNewGridPosition");
			StartCoroutine("TransitionToNewGridPosition");
		}
		else
		{
			this.WorldPosition = Gameboard.Instance.GridPositionToWorldPosition(GridPosition.x, GridPosition.y);
		}
	}

	public bool Push(Direction direction)
	{
		Rectangle newBounds = GridBounds;
		switch (direction)
		{
			case Direction.Left:
				newBounds.x -= 1;
				break;
			case Direction.Right:
				newBounds.x += 1;
				break;
			case Direction.Up:
				newBounds.y -= 1;
				break;
			case Direction.Down:
				newBounds.y += 1;
				break;
		}
		if (Gameboard.Instance.GridBounds.Contains(newBounds))
		{
			if (Gameboard.Instance.NumberOfTilesInBounds(newBounds, this) == 0)
			{
				Move(newBounds.x, newBounds.y);
				return true;
			}
		}
		return false;

	}

	// Helper Methods
	public bool IsCardinalNeighbor(GameTile tile)
	{
		bool linedUpHorizontally = (tile.GridTop >= GridTop && tile.GridTop < GridBottom) || (tile.GridBottom < GridTop && tile.GridBottom >= GridBottom);
		bool linedUpVertically = (tile.GridLeft >= GridLeft && tile.GridLeft < GridRight) || (tile.GridRight < GridLeft && tile.GridRight >= GridRight);

		bool touchingHorizontally = linedUpHorizontally && (GridLeft == tile.GridRight || GridRight == tile.GridLeft);
		bool touchingVertically = linedUpVertically && (GridTop == tile.GridBottom || GridBottom == tile.GridTop);

		return touchingHorizontally || touchingVertically;
	}

	// Coroutines
	IEnumerator FallToTarget()
	{
		_moving = true;
		float yTarget = Gameboard.Instance.GridPositionToWorldPosition(GridLeft, GridTop).y;
		bool reachedTarget = false;
		while (!reachedTarget)
		{
			_velocity.y -= Acceleration * Time.deltaTime * 60;
			WorldTop += _velocity.y * Time.deltaTime;
			if (WorldTop <= yTarget)
			{
				_velocity.y = 0;
				WorldTop = yTarget;
				reachedTarget = true;
			}
			yield return 0;
		}
		_moving = false;
		if (SettledFromFall != null)
			SettledFromFall(this);
	}
	IEnumerator TransitionToNewGridPosition()
	{
		_moving = true;
		float timer = 0f;
		Vector3 startPosition = this.transform.position;
		Vector3 endPosition = Gameboard.Instance.GridPositionToWorldPosition(GridPosition.x, GridPosition.y);
		endPosition.z = this.transform.position.z;
		endPosition.x += 0.5f * Width;
		endPosition.y -= 0.5f * Height;

		while (timer < MoveTime)
		{
			timer += Time.deltaTime;
			this.transform.position = Vector3.Lerp(startPosition, endPosition, timer * (1f / MoveTime));
			yield return null;
		}
		_moving = false;
		if (SettledFromMove != null)
			SettledFromMove(this);
		if (Pushable)
			ApplyGravity();
	}

    public void Hide()
    {
        _renderer.enabled = false;
    }
}
