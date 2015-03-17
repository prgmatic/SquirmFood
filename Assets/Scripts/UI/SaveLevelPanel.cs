using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveLevelPanel : MonoBehaviour
{
    public InputField LevelNameInput;

    PlayMakerFSM fsm;

    void Awake()
    {
        fsm = GetComponent<PlayMakerFSM>();
    }

    void OnEnable()
    {
        if(Gameboard.Instance.CurrentLevel != null)
        {
            Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("InputComplete");
        }
    }

    public void Cancel()
    {
        fsm.Fsm.Event("SaveComplete");
    }

    public void SetLevelInfo()
    {
        var layout = BoardLayoutExporter.GenerateLayout();
        layout.name = LevelNameInput.text;
        layout.ID = -1;
        Gameboard.Instance.CurrentLevel = layout;
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("InputComplete");
    }
}
