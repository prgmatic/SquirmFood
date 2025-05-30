﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FocasCameraOnGameboard : MonoBehaviour
{
    public float WidthPadding = 2;
    public float HeightPadding = 2;

    private float GaemboardWidth { get { return Gameboard.Instance.Width + WidthPadding; } }
    private float GaemboardHeight { get { return Gameboard.Instance.Height + HeightPadding; } }

    private int prevScreenWidth;
    private int prevScreenHeight;
    void Start()
    {
        AdjustCameraSize();
        prevScreenHeight = Screen.height;
        prevScreenWidth = Screen.width;
    }

    void Update()
    {
        if(Screen.width != prevScreenWidth || prevScreenHeight != Screen.height)
        {
            AdjustCameraSize();
        }

        prevScreenHeight = Screen.height;
        prevScreenWidth = Screen.width;
    }

    private void AdjustCameraSize()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float gameboardRatio = GaemboardWidth / GaemboardHeight;
        if (screenRatio > gameboardRatio)
            GetComponent<Camera>().orthographicSize = GaemboardHeight / 2;
        else
            GetComponent<Camera>().orthographicSize = (float)Screen.height / Screen.width * (GaemboardWidth / 2);
    }

    

}
