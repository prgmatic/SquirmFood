using UnityEngine;
using System.Collections.Generic;

public class MudSmearController : MonoBehaviour 
{

	public static MudSmearController Instance { get { return _instance; } }
	private static MudSmearController _instance;

	public MudSmear MudSmearPrefab;

	private List<MudSmear> _smears = new List<MudSmear>();

	void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (this != _instance)
			Destroy(this.gameObject);
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			var gridPos = Utils.CursorGridPosotion;
			AddSmear(Utils.CursorGridPosotion, Direction.Right);
		}
	}

	public void AddSmear(Point point, Direction direciton, float animationTime = 1f)
	{
		AddSmear(point.x, point.y, direciton, animationTime);
	}

	public void AddSmear(int x, int y, Direction direction, float animationTime = 1f)
	{
		MudSmear smear = Instantiate(MudSmearPrefab);
		smear.transform.SetParent(this.transform);
		_smears.Add(smear);
		smear.X = x;
		smear.Y = y;
		float zRotation = 0f;
		switch(direction)
		{
			case Direction.Up:
				zRotation = 90f;
				break;
			case Direction.Left:
				zRotation = 180f;
				break;
			case Direction.Down:
				zRotation = 270f;
				break;
		}
		smear.transform.rotation = Quaternion.Euler(0, 0, zRotation);
		smear.Animate(0.2f);
	}
}
