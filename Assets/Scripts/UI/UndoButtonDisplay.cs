using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UndoButtonDisplay : MonoBehaviour
{
    private Image _buttonImage;
    private Text _displayText;

    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _displayText = GetComponentInChildren<Text>();
    }

    private void LateUpdate()
    {
        if (MoveUndoer.UndoesRemaining > 0)
            SetAlpha(1f);
        else
            SetAlpha(.4f);
        _displayText.text = MoveUndoer.UndoesRemaining.ToString();
    }

    private void SetAlpha(float alpha)
    {
        var color = _displayText.color;
        color.a = alpha;
        _displayText.color = color;
        color = _buttonImage.color;
        color.a = alpha;
        _buttonImage.color = color;
    }
}
