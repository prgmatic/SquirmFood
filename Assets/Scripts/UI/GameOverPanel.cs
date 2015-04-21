using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour 
{
    public Text GameOverText;

    public void SetGameOverMessage(string msg)
    {
        this.GameOverText.text = msg;
    }

	public void AdvanceLevel()
	{
		LevelSelectionInfo.Instance.AdvanceLevel();
		Gameboard.Instance.StartGame();
	}

}
