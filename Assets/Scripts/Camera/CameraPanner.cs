using UnityEngine;
using System.Collections;

public class CameraPanner : MonoBehaviour
{
    public static CameraPanner Instance { get; private set; }

    public delegate void EmptyEvent();
    public event EmptyEvent PanComplete;

    public float PanTime = 1;
    public float GameStartYpos;
    public float MonitorYPos;
    public float FirstGameboardYPos;
    public float GameboardHeight;

    public void WarpCameraToStart()
    {
        transform.position = transform.position.SetY(GameStartYpos);
    }

    public void PanToStart()
    {
        StartCoroutine(Pan(GameStartYpos));
    }

    public void PanToMonitor()
    {
        StartCoroutine(Pan(MonitorYPos));
    }

    public void PanToGameboard(int gameboardNumber)
    {
        float yPos = FirstGameboardYPos - (GameboardHeight * gameboardNumber);
        StartCoroutine(Pan(yPos));
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Pan(float yPos)
    {
        float startPos = transform.position.y;
        float timer = 0f;
        while (timer < PanTime)
        {
            timer += Time.deltaTime;
            float camY = Mathf.SmoothStep(startPos, yPos, timer / PanTime);
            transform.position = transform.position.SetY(camY);
            yield return null;
        }
        transform.position = transform.position.SetY(yPos);
        if (PanComplete != null)
            PanComplete();
    }

}
