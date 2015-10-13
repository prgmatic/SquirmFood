using UnityEngine;
using System.Collections;

public class ClickDetection : MonoBehaviour
{
    public LayerMask LayerMask;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CheckAtScreenPosition(Input.mousePosition);
        }
    }

    private void CheckAtScreenPosition(Vector3 position)
    {
        //var ray = Camera.main.ScreenPointToRay(position);
        //RaycastHit hitInfo;
        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    Debug.Log(hitInfo.GetComponent<Collider>().gameObject.name);
        //    var button = hitInfo.GetComponent<Collider>().GetComponent<Button>();
        //    if (button != null)
        //    {
        //        button.Click();
        //    }
        //}
    }
}
