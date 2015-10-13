using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaycastHitTest : MonoBehaviour
{
    public Camera PerspectiveCamera;
    public Camera RTCamera;
    public GameObject MonitorScreen;
    public GameObject RayVisual;
    public GraphicRaycaster Raycaster;
    public GameObject MonitorUI;

    private bool _hoveringScreen = false;
    private Vector3 _rayOrigin = Vector3.zero;

    private void Update()
    {
        _hoveringScreen = false;
        var ray = PerspectiveCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject == MonitorScreen)
            {
                Debug.Log("hit");

                _hoveringScreen = true;
                var screenPos = new Vector2(RTCamera.pixelWidth * hitInfo.textureCoord.x,
                                            RTCamera.pixelHeight * hitInfo.textureCoord.y);
                var ray2 = RTCamera.ScreenPointToRay(screenPos);
                _rayOrigin = ray2.origin;
                RayVisual.transform.position = _rayOrigin;

                var pointer = new PointerEventData(EventSystem.current);
                pointer.position = Vector3.zero;
                ExecuteEvents.Execute(MonitorUI, pointer, ExecuteEvents.pointerDownHandler);

                //Raycaster.Raycast(pointer, )

                //PointerEventData ped = new PointerEventData(EventSystem.current);
                //ped.position = _rayOrigin;
                //var results = new List<RaycastResult>();
                //Raycaster.Raycast(ped, results);
                //Debug.Log(results.Count);
            }
        }
    }
}
