using UnityEngine;
using System.Collections;

public class LevelBuildingOptions : MonoBehaviour
{
    PlayMakerFSM fsm;
    HutongGames.PlayMaker.FsmState savingLevel;

    void Awake()
    {
        fsm = GetComponent<PlayMakerFSM>();
        savingLevel = fsm.Fsm.GetState("SavingLevel");
        WebManager.Instance.SaveComplete += Insstance_SaveComplete;
    }

    private void Insstance_SaveComplete(string levelName, int id)
    {
        //Gameboard.Instance.CurrentLevel = BoardLayoutExporter.GenerateLayout();
        //Gameboard.Instance.CurrentLevel.name = levelName;
        //Gameboard.Instance.CurrentLevel.ID = id;
        

        //Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("SaveComplete");
    }

    public void SaveLevel()
    {
        if(fsm.Fsm.ActiveState == savingLevel)
        {
            WebManager.Instance.SaveLevel(Gameboard.Instance.CurrentLevel.name, BoardLayoutExporter.ExportBinary(), Gameboard.Instance.CurrentLevel.ID);
        }
    }
}
