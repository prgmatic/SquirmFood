using UnityEngine;
using System.Collections;

public class LevelInfoPanner : MonoBehaviour
{
    private const float ADJUSTMENT = -7.2f;
    private const float LAYER_HEIGHT = 22f;
    private const float CANVAS_OFFSET = -8.2f;

    public Transform CameraRig;
    public LevelInfoView LevelView0;
    public LevelInfoView LevelView1;

    private int _previousLayer = 0;
    private int _layersDown
    {
        get
        {
            return Mathf.Max(0, -(int)((CameraRig.position.y - ADJUSTMENT) / LAYER_HEIGHT));
        }
    }

    private void Update()
    {
        if (_layersDown != _previousLayer)
        {
            UpdateInfo();
        }
        _previousLayer = _layersDown;
    }
    public void UpdateInfo()
    {
        LevelView0.SetLevel(_layersDown);
        LevelView1.SetLevel(_layersDown + 1);
        this.transform.position = this.transform.position.SetY(_layersDown * -LAYER_HEIGHT);
    }

    private void Start()
    {
        UpdateInfo();
    }
}
