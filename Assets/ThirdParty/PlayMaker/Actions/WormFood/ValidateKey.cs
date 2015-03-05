using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Tooltip = HutongGames.PlayMaker.TooltipAttribute;

[ActionCategory("Worm Food")]
[Tooltip("Validate the users key.")]
public class ValidateKey : FsmStateAction
{

    public FsmEvent ValidatedEvent;
    public FsmEvent InvalidatedEvent;
    public bool SkipValidation;


    public override void Reset()
    {
        base.Reset();
        ValidatedEvent = null;
        InvalidatedEvent = null;
        SkipValidation = false;
    }

    public override void OnEnter()
    {

        if(SkipValidation)
        {
            Fsm.Event(ValidatedEvent);
        }
        else
        {
            if (RequestParameters.HasKey("key"))
            {
                string key = RequestParameters.GetValue("key");
                PlaythroughDatabase.Insstance.KeyValidationComplete += Insstance_KeyValidationComplete;
                PlaythroughDatabase.Insstance.ValidateKey(key);
            }
            else Fsm.Event(InvalidatedEvent);
        }
    }

    private void Insstance_KeyValidationComplete(bool keyValidated, string testerName)
    {
        if (keyValidated)
            Fsm.Event(ValidatedEvent);
        else
            Fsm.Event(InvalidatedEvent);
    }

}
