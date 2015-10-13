using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DeleteMe : MonoBehaviour {

    public GameObject MonitorUI;

	void Update()
    {
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Vector3.zero;
        pointer.button = PointerEventData.InputButton.Left;

        var newPress = ExecuteEvents.ExecuteHierarchy(MonitorUI, pointer, ExecuteEvents.pointerDownHandler);
        if (newPress != null)
            Debug.Log(newPress.name);
        else Debug.Log("Poop");

        ExecuteEvents.Execute(MonitorUI, pointer, ExecuteEvents.pointerDownHandler);
    }
}
