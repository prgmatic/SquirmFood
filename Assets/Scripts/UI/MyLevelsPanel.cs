using UnityEngine;
using System.Collections;

public class MyLevelsPanel : MonoBehaviour {


    public void GoToMenu()
    {
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("GoToMenu");

    }

    public void NewLevel()
    {
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("CreateNewLevel");
    }
}
