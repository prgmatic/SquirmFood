using UnityEngine;
using System.Collections;

public class NewMudRenderer : MonoBehaviour
{
    public MudRenderer OldRenderer;
    private SpriteRenderer _renderer;
    private Material _mudMaterial;

    private void Awake()
    {
        Gameboard.Instance.LevelChanged += Instance_LevelChanged;
        _renderer = GetComponent<SpriteRenderer>();
        _mudMaterial = _renderer.material;
    }

    private void Instance_LevelChanged(NewBoardLayout level)
    {
        SetLevel(level);
    }

    public void SetLevel(NewBoardLayout level)
    {

        if(level.MudMask != null)
        {
            _mudMaterial.SetTexture("_MaskTex", level.MudMask.texture);
            Show();
            OldRenderer.Hide();
        }
        else
        {
            Hide();
            OldRenderer.Show();
        }
    }

    private void Show()
    {
        _renderer.enabled = true;
    }
    private void Hide()
    {
        _renderer.enabled = false;
    }
}
