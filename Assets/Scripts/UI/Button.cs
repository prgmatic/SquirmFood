using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
	public virtual void Click()
    {
        Debug.Log("this button has been clicked!");
    }
}
