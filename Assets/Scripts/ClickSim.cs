using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickSim : MonoBehaviour {

    public GameObject Canvas;
    public GraphicRaycaster Raycaster;

	public void Update()
    {
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Vector3.zero;
        //ExecuteEvents.Execute(Canvas, pointer, ExecuteEvents.submitHandler);

        var results = new List<RaycastResult>();
        Raycaster.Raycast(pointer, results);
        Debug.Log(results.Count);

    }
}
