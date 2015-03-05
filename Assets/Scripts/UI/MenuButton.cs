using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour
{
    public void GoToMenu()
    {
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("GoToMenu");
        //Gameboard.Instance.ShowLevelSelect();
    }
}
