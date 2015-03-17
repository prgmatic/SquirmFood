using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using Tooltip = HutongGames.PlayMaker.TooltipAttribute;

[ActionCategory("Worm Food")]
[Tooltip("Validate the users key.")]
public class ValidateKey : FsmStateAction
{
    public static string KeyHolderName = "";
    public static string Key = "";
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
            if (RequestParameters.IsInitialized)
            {
                DoKeyValidation();
            }
            else
            {
                RequestParameters.Initialized += RequestParameters_Initialized;
            }
        }
    }

    private void RequestParameters_Initialized(object sender, System.EventArgs e)
    {
        RequestParameters.Initialized -= RequestParameters_Initialized;
        DoKeyValidation();
    }

    private void DoKeyValidation()
    {
        if (RequestParameters.HasKey("key"))
        {
            Key = RequestParameters.GetValue("key");
            WebManager.Instance.KeyValidationComplete += Insstance_KeyValidationComplete;
            WebManager.Instance.CheckKeyValidation(Key);
        }
        else Fsm.Event(InvalidatedEvent);
    }

    private void Insstance_KeyValidationComplete(bool keyValidated, string testerName)
    {
        if (keyValidated)
        {
            KeyHolderName = testerName;
            Fsm.Event(ValidatedEvent);
        }
        else
            Fsm.Event(InvalidatedEvent);
    }

}
