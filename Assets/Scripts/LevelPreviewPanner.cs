using UnityEngine;
using System.Collections;

public class LevelPreviewPanner : MonoBehaviour
{
    private const float ADJUSTMENT = -7.2f;
    private const float LAYER_HEIGHT = 22f;
    private const float CANVAS_OFFSET = -8.2f;

    public Transform CameraRig;
    public LevelPreview LevelPreview0;
    public LevelPreview LevelPreview1;

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
            UpdatePreview();
        }
        _previousLayer = _layersDown;
    }

    private void SetLayer(int layer)
    {
        LevelPreview0.LoadLevel(layer);
        LevelPreview1.LoadLevel(layer + 1);
    }

    public void UpdatePreview()
    {
        SetLayer(_layersDown);
        this.transform.position = this.transform.position.SetY(_layersDown * -LAYER_HEIGHT);
    }

    private void Start()
    {
        UpdatePreview();
    }
}