using UnityEngine;
using System.Collections;

public class FsmEventTrigger : MonoBehaviour
{
    PlayMakerFSM fsm;
    void Awake()
    {
        fsm = GetComponent<PlayMakerFSM>();
    }

    public void Event(string eventName)
    {
        fsm.Fsm.Event(eventName);
    }
}
