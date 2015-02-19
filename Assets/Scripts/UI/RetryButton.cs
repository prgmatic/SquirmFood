using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour {

	public void Retry()
    {
        Gameboard.Instance.Retry();
    }
}
