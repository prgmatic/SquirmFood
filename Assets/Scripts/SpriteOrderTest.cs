using UnityEngine;
using System.Collections.Generic;

public class SpriteOrderTest : MonoBehaviour 
{
	public int SortingOrder = 0;

	private SpriteRenderer _renderer;
	public void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void Update()
	{
		var gridPos = Gameboard.Instance.WorldPositionToGridPosition(this.transform.position);
		_renderer.sortingOrder = gridPos.y * 10 + 15;
	}
}
