using UnityEngine;
using System.Collections;

public class StayWithCamera : MonoBehaviour {

    public GameObject Camera;
    public Transform MidLayer;
    public Transform BottomLayer;

    public float Adjustment = -7.2f;


    public float LAYER_HEIGHT = 24f;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = this.transform.position;
    }

    private void Update()
    {
        int layersDown = -(int)((Camera.transform.position.y - Adjustment) / LAYER_HEIGHT);
        layersDown -= 1;
        layersDown = Mathf.Max(0, layersDown);
        //Debug.Log(layersDown);
        this.transform.position = this.transform.position.SetY(layersDown * -LAYER_HEIGHT);
        //if(layersDown >= 0)
        //{
        //}
    }
}
