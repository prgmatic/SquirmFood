using UnityEngine;
using System.Collections.Generic;

public class WinCondition : MonoBehaviour 
{
	private bool GameWon { get { return NumberOfBodyPartsLeft() == 0; } }

	void Awake()
	{
		Gameboard.Instance.TileDestroyed += Instance_TileDestroyed;
		//if (GameWon) DoGameWon();
	}

	private void Instance_TileDestroyed(GameTile sender)
	{
		if (GameWon) DoGameWon();
	}


	private int NumberOfBodyPartsLeft()
	{
		int result = 0;
		var gameboard = Gameboard.Instance;
		foreach(var tile in gameboard.gameTiles)
		{
			if(tile != null)
			{
				if (tile.Width > 1 || tile.Height > 1)
					result++;
			}
		}
		return result;
	}

	private void DoGameWon()
	{
		Gameboard.Instance.EndGame();
        GameManager.Instance.LevelComplete();
	}
}
