using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingLevelTitle : MonoBehaviour
{
    public LevelTitleView StaticTitle;
    public LevelTitleView ScrollingTitle;
    public float MinAlpha = .5f;

    private Vector3 _staticStartPosition;
    private Vector3 _scrollingStartPosition;
    private float _scrollDistance;            // The distance the scrolling title needs to scroll before it has reached the position of where the static title would be located

    private int _currentLayer { get { return CameraPanner.Instance.CurrentLayer; } }
    private float _layerHeight { get { return CameraPanner.Instance.LayerHeight; } }

    private void Start()
    {
        _staticStartPosition = StaticTitle.transform.position;
        _scrollingStartPosition = ScrollingTitle.transform.position;
        _scrollDistance = _scrollingStartPosition.y - (_staticStartPosition.y - _layerHeight);
    }
    private void LateUpdate()
    {
        PositionScrollingTitle();
    }
    private void PositionScrollingTitle()
    {
        var camPos = CameraPanner.Instance.YPos;

        camPos -= CameraPanner.Instance.FirstGameboardYPos;
        var yPos = _scrollingStartPosition.y - _currentLayer * _layerHeight;
        var offset = -camPos % _layerHeight;
        offset = Mathf.Max(0, offset);

        // if scrolling text has not reached the location of where the static title will be
        if(offset < _scrollDistance)
        {
            StaticTitle.transform.position = StaticTitle.transform.position.SetY(_staticStartPosition.y - (_currentLayer) * _layerHeight);
            yPos -= offset;
            var zPos = GetScrollingDepth(offset / _scrollDistance);
            SetScrollingTitlePosition(yPos, zPos);

            StaticTitle.SetLevel(GameManager.Instance.GetLevel(_currentLayer));
            ScrollingTitle.SetLevel(GameManager.Instance.GetLevel(_currentLayer + 1));
            ScrollingTitle.SetAlpha(Mathf.Lerp(MinAlpha, 1f, offset / _scrollDistance));
        }
        else
        {
            StaticTitle.transform.position = StaticTitle.transform.position.SetY(_staticStartPosition.y - (_currentLayer + 1) * _layerHeight);
            SetScrollingTitlePosition(_scrollingStartPosition.y - (_currentLayer + 1) * _layerHeight, _scrollingStartPosition.z);

            StaticTitle.SetLevel(GameManager.Instance.GetLevel(_currentLayer + 1));
            ScrollingTitle.SetLevel(GameManager.Instance.GetLevel(_currentLayer + 2));
            ScrollingTitle.SetAlpha(MinAlpha);
        }
    }
    private float GetScrollingDepth(float delta)
    {
        return Mathf.Lerp(_scrollingStartPosition.z, _staticStartPosition.z, delta);
    }
    private void SetScrollingTitlePosition(float yPos, float zPos)
    {
        ScrollingTitle.transform.position = new Vector3(0, yPos, zPos);
    }
    //private string GetLevelTitle(int levelNumber)
    //{
    //    var level = GameManager.Instance.LevelSet.Levels[levelNumber];
    //    if (level != null)
    //        return level.name;
    //    return string.Empty;
    //}
}
