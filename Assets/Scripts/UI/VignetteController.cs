using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VignetteController : MonoBehaviour
{
    public float FadeTime = 0.3f;
    public float Alpha
    {
        get { return _image.color.a; }
        set
        {
            var color = _image.color;
            color.a = value;
            _image.color = color;
        }
    }
    private bool _fading = false;

    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
        Alpha = 0;
        GameManager.Instance.StateChanged += Instance_StateChanged;
    }


    private void LateUpdate()
    {
        if (_fading) return;
        var distFromMonitorToFirstLevel = CameraPanner.Instance.MonitorYPos - CameraPanner.Instance.FirstGameboardYPos;
        if(GameManager.Instance.State != GameManager.GameState.PlayingGame)
        {
            Alpha = Mathf.Lerp(0, 1, (-CameraPanner.Instance.YPos + CameraPanner.Instance.MonitorYPos) / distFromMonitorToFirstLevel);
        }
    }

    private void Instance_StateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PlayingGame)
            FadeOut();
        else
            FadeIn();
    }


    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        _fading = true;
        float start = _image.color.a;
        float time = start * FadeTime;
        while (time < FadeTime)
        {
            time += Time.deltaTime;
            Alpha = Mathf.Lerp(0, 1, time / FadeTime);
            yield return null;
        }
        _fading = false;
        Alpha = 1f;
    }
    private IEnumerator FadeOutRoutine()
    {
        _fading = true;
        float start = _image.color.a;
        float time = (1f - start) * FadeTime;
        while (time < FadeTime)
        {
            time += Time.deltaTime;
            Alpha = Mathf.Lerp(1, 0, time/ FadeTime);
            yield return null;
        }
        _fading = false;
        Alpha = 0f;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("FadeIn"))
        {
            FadeIn();
        }
        if(GUILayout.Button("FadeOut"))
        {
            FadeOut();
        }
    }
}
