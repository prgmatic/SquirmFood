using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Tooltip = HutongGames.PlayMaker.TooltipAttribute;

[ActionCategory("Worm Food")]
[Tooltip("End Game")]
public class EndGame : FsmStateAction
{
    public override void OnEnter()
    {
        base.OnEnter();
        Gameboard.Instance.EndGame();
        Finish();
    }
}