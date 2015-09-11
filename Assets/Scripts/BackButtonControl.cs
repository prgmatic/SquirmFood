using UnityEngine;
using System.Collections;

public class BackButtonControl : MonoBehaviour 
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Kill it");
            Application.Quit();
        }
    }
}
