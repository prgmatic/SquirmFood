using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour
{
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Retry();
    }
	public void Retry()
    {
        Gameboard.Instance.Retry();
    }
}
