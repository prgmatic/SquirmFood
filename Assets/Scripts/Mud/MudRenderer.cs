using UnityEngine;
using System.Collections.Generic;

public class MudRenderer : MonoBehaviour
{
	public Color MudColor = Color.white;
	private SpriteRenderer[,] _mudRenderers;

	void Awake()
	{
		var gameboard = GetComponent<Gameboard>();
		gameboard.TileAttributeChanged += Gameboard_TileAttributeChanged;
		InitMudTiles(gameboard);
	}

    public void Show()
    {
        foreach (var renderer in _mudRenderers)
            renderer.gameObject.SetActive(true);
    }
    public void Hide()
    {
        foreach (var renderer in _mudRenderers)
            renderer.gameObject.SetActive(false);
    }

    private void InitMudTiles(Gameboard gameboard)
	{
		var parent = new GameObject();
		parent.name = "MudTiles";
		parent.transform.SetParent(this.transform);
		parent.layer = this.gameObject.layer;
		_mudRenderers = new SpriteRenderer[gameboard.Columns, gameboard.Rows];
		for (int y = 0; y < gameboard.Rows; y++)
		{
			for (int x = 0; x < gameboard.Columns; x++)
			{
				var go = new GameObject();
				go.name = "MudTile";
				go.layer = parent.layer;
				go.transform.SetParent(parent.transform);
				go.transform.position = gameboard.GridPositionToWorldPosition(x, y) + new Vector3(0.5f, -0.5f);
				_mudRenderers[x, y] = go.AddComponent<SpriteRenderer>();
				_mudRenderers[x, y].sprite = Utils.DummySprite;
				_mudRenderers[x, y].sortingOrder = -50;
				_mudRenderers[x, y].color = MudColor;
				_mudRenderers[x, y].enabled = false;
			}
		}
	}

	private void Gameboard_TileAttributeChanged(int x, int y, Gameboard.BackgroundTileAttribute attribute)
	{
		_mudRenderers[x, y].enabled = attribute == Gameboard.BackgroundTileAttribute.FreeMove;
	}
}
