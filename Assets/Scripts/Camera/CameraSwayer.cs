using UnityEngine;
using System.Collections;

public class CameraSwayer : MonoBehaviour
{
    public float XScale = 1.0f;
    public float YScale = 1.0f;
    public float XSpeed = 1.0f;
    public float YSpeed = 1.0f;

    void Update()
    {
        float xPos = XScale * Mathf.PerlinNoise(Time.time * XScale, 0);
        float yPos = YScale * Mathf.PerlinNoise(0, Time.time * YScale);
        this.transform.localPosition = new Vector3(xPos, yPos, 0);
    }
}