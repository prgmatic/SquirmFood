using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using System.Reflection;
using Tooltip = HutongGames.PlayMaker.TooltipAttribute;
using System.Linq;

[ActionCategory("Worm Food")]
[Tooltip("Calls Method on Behavior")]
public class CallMethod : FsmStateAction
{
    [RequiredField]
    public FsmOwnerDefault GameObject;

    [RequiredField]
    public string BehaviourName = "";
    [RequiredField]
    public string MethodName = "";

    Component component;
    MethodInfo method;

    public override void Reset()
    {
        base.Reset();
        GameObject = null;
        BehaviourName = "";
        MethodName = "";
    }

    public override void Awake()
    {
        if (Application.isPlaying)
        {
            if (BehaviourName.Length > 0 && MethodName.Length > 0)
            {
                component = this.Fsm.GetOwnerDefaultTarget(this.GameObject).GetComponent(BehaviourName);
                if (component != null)
                {
                    method = component.GetType().GetMethod(MethodName,
                        BindingFlags.Public |
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Instance);
                }
            }
        }
    }

    public override void OnEnter()
    {
        if(method != null)
        {
            method.Invoke(component, null);
        }
        Finish();
    }
}
