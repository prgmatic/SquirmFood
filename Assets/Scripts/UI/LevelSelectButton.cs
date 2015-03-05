using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HutongGames.PlayMaker;

public class LevelSelectButton : MonoBehaviour
{
    public Text ButtonText;

    [System.NonSerialized]
    public BoardLayout Layout;

    public void Select()
    {
        PlayMakerFSM fsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
        BoardLayoutSet bls = Gameboard.Instance.GetComponent<BoardLayoutSet>();
        bls.SetLayout(Layout);
        fsm.Fsm.Event("LevelSelected");
       
        /*
        FsmInt level = FsmVariables.GlobalVariables.GetFsmInt("Level");
        level.Value = 10; 
        HutongGames.PlayMaker.FsmVariables.GlobalVariables.set


        if(Gameboard.Instance.GetComponent<BoardLayoutSet>() != null)
        {
            if(Layout != null)
            {
                LevelSelectionPanel.Instance.Hide();
                Gameboard.Instance.Show();
                Gameboard.Instance.GetComponent<BoardLayoutSet>().SetLayout(Layout);
                Gameboard.Instance.StartGame();
            }
        }
        */
    }
}
