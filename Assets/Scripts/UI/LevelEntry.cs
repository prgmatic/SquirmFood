using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEntry : MonoBehaviour
{
    [System.NonSerialized]
    public int LevelID = -1;
    [SerializeField]
    private Text _text = null;
    public string LevelName { set { _text.text = value; } }

    public void LoadLevel()
    {
        WebManager.Instance.GetLevel(LevelID, ValidateKey.Key);
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Variables.GetFsmString("LevelOption").Value = "Build";
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("LevelSelected");
    }

    public void TestLevel()
    {
        WebManager.Instance.GetLevel(LevelID, ValidateKey.Key);
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Variables.GetFsmString("LevelOption").Value = "Test";
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("LevelSelected");
    }

    public void SubmitLevel()
    {
        WebManager.Instance.GetLevel(LevelID, ValidateKey.Key);
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Variables.GetFsmString("LevelOption").Value = "Submit";
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("LevelSelected");
    }

}