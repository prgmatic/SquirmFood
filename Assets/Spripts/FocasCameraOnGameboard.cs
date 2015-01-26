using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FocasCameraOnGameboard : MonoBehaviour
{
    public Gameboard gameboard;
    private int prevScreenWidth;
    private int prevScreenHeight;
    private ScreenOrientation previousOrientation;
    void Start()
    {
        Debug.Log(gameboard.Width);
        AdjustCameraSize();
        prevScreenHeight = Screen.height;
        prevScreenWidth = Screen.width;
        previousOrientation = Screen.orientation;
    }

    void Update()
    {
        if(Screen.width != prevScreenWidth || prevScreenHeight != Screen.height)
        {
            AdjustCameraSize();
        }

        prevScreenHeight = Screen.height;
        prevScreenWidth = Screen.width;
        previousOrientation = Screen.orientation;
    }

    private void AdjustCameraSize()
    {
        if (Screen.width > Screen.height)
            camera.orthographicSize = gameboard.Height / 2;
        else
            camera.orthographicSize = (float)Screen.height / Screen.width * (gameboard.Width / 2);
    }

    

}
