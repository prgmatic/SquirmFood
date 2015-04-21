using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HutongGames.PlayMaker;

public class LevelSelectButton : MonoBehaviour
{
	public delegate void SelectLevelEvent(BoardLayout level);
	public event SelectLevelEvent LevelSelected;

    public Text ButtonText;

    [System.NonSerialized]
    public BoardLayout Layout;

    public void Select()
    {
		/*
        PlayMakerFSM fsm = Gameboard.Instance.GetComponent<PlayMakerFSM>();
        BoardLayoutSet bls = Gameboard.Instance.GetComponent<BoardLayoutSet>();
        bls.SetLayout(Layout);
        fsm.Fsm.Event("LevelSelected");
		*/
		Debug.Log("Level selected");
		if(LevelSelected != null)
		{
			LevelSelected(Layout);
		}
    }
}
