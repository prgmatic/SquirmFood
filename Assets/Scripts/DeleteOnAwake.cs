using UnityEngine;
using System.Collections;

public class DeleteOnAwake : MonoBehaviour {

	private void Awake()
    {
        Destroy(this.gameObject);
    }
}
