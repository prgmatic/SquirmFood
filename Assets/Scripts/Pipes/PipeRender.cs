using UnityEngine;
using System.Collections.Generic;

public class PipeRender : MonoBehaviour
{
	private List<PipeObject> _pipes = new List<PipeObject>();
	private Gameboard _gameboard;

	void Awake()
	{
		_gameboard = Gameboard.Instance;
		_gameboard.TileAdded += Instance_TileAdded;
		_gameboard.TileDestroyed += Instance_TileDestroyed;
		AddPipeObjects();
	}

	private void Instance_TileDestroyed(GameTile sender)
	{
		if (sender.IsWall)
		{
			UpdatePipes();
		}
	}

	private void Instance_TileAdded(GameTile sender)
	{
		UpdatePipes();
	}

	private void UpdatePipes()
	{
		foreach (var pipe in _pipes)
		{
			UpdatePipe(pipe);
		}
	}

	private void UpdatePipe(PipeObject po)
	{
		bool top = IsWall(po.X, po.Y - 1);
		bool bottom = IsWall(po.X, po.Y + 1);
		bool left = IsWall(po.X - 1, po.Y);
		bool right = IsWall(po.X + 1, po.Y);

		po.UpdatePipe(IsWall(po.X, po.Y), top, bottom, left, right);
	}

	private bool IsWall(int x, int y)
	{
		var gameboar = Gameboard.Instance;
		if (!Gameboard.Instance.IsValidTileCoordinate(x, y)) return false;
		var tile = _gameboard.GetTileAt(x, y);
		return tile != null && tile.IsWall;
	}

	private void AddPipeObjects()
	{
		GameObject pipesGO = new GameObject();
		pipesGO.name = "Pipes";
		pipesGO.transform.SetParent(this.transform);
		pipesGO.layer = this.gameObject.layer;

		for (int y = 0; y < _gameboard.Rows; y++)
		{
			for (int x = 0; x < _gameboard.Columns; x++)
			{
				GameObject go = new GameObject();
				go.name = "Pipe";
				go.transform.SetParent(pipesGO.transform);
				go.layer = pipesGO.layer;
				var po = go.AddComponent<PipeObject>();
				po.transform.position = Gameboard.Instance.GridPositionToWorldPosition(x, y) + new Vector3(0.5f, -0.5f, 0f);
				po.transform.rotation = Quaternion.Euler(180, 0, 0);
				po.X = x;
				po.Y = y;
				_pipes.Add(po);
			}
		}
	}
}
