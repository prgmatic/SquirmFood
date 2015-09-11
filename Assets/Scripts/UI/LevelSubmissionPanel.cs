using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSubmissionPanel : MonoBehaviour
{
    public InputField LevelName;
    public InputField AuthorName;
    void OnEnable()
    {
        LevelName.text = Gameboard.Instance.CurrentLevel.name;
        AuthorName.text = ValidateKey.KeyHolderName;
    }

    public void Submit()
    {
        //Playthrough playthrough = Gameboard.Instance.GetComponent<PlaythroughRecorder>().ExportPlaythrough();
        //playthrough.TesterName = AuthorName.text;
        //byte[] levelData = BoardLayoutExporter.ExportBinary(Gameboard.Instance.CurrentLevel);
        //WebManager.Instance.SubmitLevel(LevelName.text, levelData, playthrough);
        //Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("LevelSubmitted");
    }
	
}
