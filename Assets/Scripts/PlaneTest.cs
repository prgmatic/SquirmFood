using UnityEngine;
using System.Collections.Generic;

public class PlaneTest : MonoBehaviour
{
	public Sprite SelectionSprite;
	private Plane _plane;
	private SpriteRenderer _renderer;

	void Awake()
	{
		GameObject go = new GameObject();
		go.name = "Tile Selection";
		go.transform.SetParent(this.transform);
		go.layer = this.gameObject.layer;
		_renderer = go.AddComponent<SpriteRenderer>();
		_renderer.sprite = SelectionSprite;
		_renderer.transform.localScale = new Vector3(1f / SelectionSprite.bounds.size.x, 1f / SelectionSprite.bounds.size.y, 1f);
		_renderer.enabled = false;
		_plane = new Plane(Vector3.forward, 0f);
	}

	void OnDisable()
	{
		_renderer.enabled = false;
	}

	void Update()
	{
		var pos = Utils.CursorPositionInWorld;
		var gridPos = Gameboard.Instance.WorldPositionToGridPosition(pos);
		if (Gameboard.Instance.IsValidTileCoordinate(gridPos))
		{
			_renderer.enabled = true;
			pos.x = Mathf.FloorToInt(pos.x) + 0.5f;
			pos.y = Mathf.FloorToInt(pos.y) + 0.5f;
			_renderer.transform.position = pos;
		}
		else _renderer.enabled = false;
	}
}
