using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogPlaythroughPanel : MonoBehaviour
{
    public StarRating DifficultyRating;
    public StarRating SatisfactionRating;
    public InputField TesterName;
    public InputField Notes;
    

    public void Show()
    {
        this.gameObject.SetActive(true);
        DifficultyRating.SetRating(3);
        DifficultyRating.SetRating(3);
        TesterName.text = Gameboard.Instance.GetComponent<BoardLayoutSet>().PlayTesterName;
        Notes.text = "";
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Submit()
    {
        BoardLayoutSet layouts = Gameboard.Instance.GetComponent<BoardLayoutSet>();
        layouts.AddPlaythrough(TesterName.text, Notes.text, DifficultyRating.Rating, SatisfactionRating.Rating);
        Hide();
        UIGlobals.Instance.GameOverPanel.Show("");
    }
}
