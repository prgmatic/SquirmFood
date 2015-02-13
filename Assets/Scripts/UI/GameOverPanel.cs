using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverPanel : MonoBehaviour 
{
    public Text GameOverText;
    public Button NextLevelButton;

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(string gameOverMsg)
    {
        this.gameObject.SetActive(true);
        this.GameOverText.text = gameOverMsg;

        BoardLayoutSet bls = Gameboard.Instance.GetComponent<BoardLayoutSet>();
        if (bls != null && bls.enabled)
        {
            NextLevelButton.gameObject.SetActive(true);
        }
        else NextLevelButton.gameObject.SetActive(false);
    }

    
}
