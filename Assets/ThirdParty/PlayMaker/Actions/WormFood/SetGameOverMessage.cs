using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Tooltip = HutongGames.PlayMaker.TooltipAttribute;

[ActionCategory("Worm Food")]
[Tooltip("Set Game Over Message")]
public class SetGameOverMessage : FsmStateAction
{
    [RequiredField]
    public GameOverPanel GameOverPanel;
    [RequiredField]
    public FsmString Message;


    public override void Reset()
    {
        GameOverPanel = null;
        Message = "";
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameOverPanel.SetGameOverMessage(Message.Value);
        Finish();
    }
}