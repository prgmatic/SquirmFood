using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Tooltip = HutongGames.PlayMaker.TooltipAttribute;

[ActionCategory("Worm Food")]
[Tooltip("Start Game")]
public class StartGame : FsmStateAction
{

    public override void OnEnter()
    {
        base.OnEnter();
        Gameboard.Instance.StartGame();
        Finish();
    }
}