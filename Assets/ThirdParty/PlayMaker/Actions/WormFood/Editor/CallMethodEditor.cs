using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using System.Reflection;
using System.Linq;

[CustomActionEditor(typeof(CallMethod))]
public class CallMethodEditor : CustomActionEditor
{
    string[] listOfMethods;
    List<MethodInfo> methods = new List<MethodInfo>();
    //int selectedBehaviour = -1;
    int selectedMethod = -1;

    GameObject prevSelectedGameObject = null;
    //int prevSelectedBehaviour = -1;

    CallMethod callMethod;

    public override void OnEnable()
    {
        if (target != null)
        {
            callMethod = (CallMethod)target;
            if (callMethod.GameObject != null)
            {
                GameObject go = callMethod.Fsm.GetOwnerDefaultTarget(callMethod.GameObject);
                if (go != null)
                {
                    GetMethodsForGameObject(go);

                    if (callMethod.BehaviourName != null
                        && callMethod.BehaviourName.Length > 0
                        && callMethod.MethodName != null
                        && callMethod.MethodName.Length > 0)
                    {
                        for (int i = 0; i < methods.Count; i++)
                        {
                            if (callMethod.BehaviourName == methods[i].DeclaringType.Name && callMethod.MethodName == methods[i].Name)
                            {
                                selectedMethod = i;
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (selectedMethod < 0)
        {
            callMethod.BehaviourName = "";
            callMethod.MethodName = "";
        }
    }

    public override bool OnGUI()
    {
        if (callMethod == null || callMethod.GameObject == null) return false;

        if (callMethod.BehaviourName == "" || callMethod.MethodName == "")
        {
            selectedMethod = -1;
        }

        var go = callMethod.Fsm.GetOwnerDefaultTarget(callMethod.GameObject);

        EditField("GameObject");
        if (go != null)
        {
                if (prevSelectedGameObject != go)
                {
                    GetMethodsForGameObject(go);
                }
                selectedMethod = EditorGUILayout.Popup("Method", selectedMethod, listOfMethods);
        }
        callMethod.BehaviourName = "";
        callMethod.MethodName = "";
        if(selectedMethod > -1)
        {
            callMethod.BehaviourName = methods[selectedMethod].DeclaringType.Name;
            callMethod.MethodName = methods[selectedMethod].Name;
        }
        prevSelectedGameObject = go;

        
                
        return GUI.changed;
    }

    void GetMethodsForGameObject(GameObject go)
    {
        methods.Clear();

        MonoBehaviour[] behaviours = go.GetComponents<MonoBehaviour>();
        behaviours = behaviours.OrderBy(b => b.name).ToArray();

        foreach (var behaviour in behaviours)
        {
            var behaviourMethods = behaviour.GetType().GetMethods(
                BindingFlags.Public |
                BindingFlags.DeclaredOnly |
                BindingFlags.Instance)
                .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.ReturnType == typeof(void) && m.GetParameters().Length == 0)
                .Select(m => m).OrderBy(m=>m.Name).ToArray();

            methods.AddRange(behaviourMethods);
        }

        listOfMethods = new string[methods.Count];
        for(int i = 0; i < methods.Count; i++)
        {
            listOfMethods[i] = methods[i].DeclaringType + "/" + methods[i].Name + "()";
        }
    }
}
