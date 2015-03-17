using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogPlaythroughPanel : MonoBehaviour
{
    public StarRating DifficultyRating;
    public StarRating SatisfactionRating;
    public InputField TesterName;
    public InputField Notes;
    
    void OnEnable()
    {
        DifficultyRating.SetRating(1);
        SatisfactionRating.SetRating(1);
        TesterName.text = ValidateKey.KeyHolderName;
        Notes.text = "";
    }

    public void Submit()
    {
        PlaythroughRecorder recorder = Gameboard.Instance.GetComponent<PlaythroughRecorder>();
        recorder.SubmitPlaythrough(TesterName.text, Notes.text, DifficultyRating.Rating, SatisfactionRating.Rating);
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("PlaythroughSubmitted");
        //Hide();
        //UIGlobals.Instance.GameOverPanel.Show("");
    }
}
