using UnityEngine;
using System.Collections;

public class CameraLevelSelect : MonoBehaviour
{
    public float TopClamp = -7.2f;
    public float BottomClamp = 100f;
    public float LayerHeight = 22f;
    public float ScrollSensitivity = 1f;
    public float DecelarationRate = 0.135f;
    public float Elasticity = .1f;
    public float ViewSize = 1.0f;
    public float SnapSpeed = .3f;
    public float SnapVelocity = 20f;

    private Vector2 _previousSwipePosition;
    private float _velocity = 0f;
    private float _previousYPos = 0f;
    private bool _snapping = false;

    private float _yPos
    {
        get { return this.transform.position.y; }
        set
        {
            this.transform.position = this.transform.position.SetY(value);
        }
    }
    private float deltaTime
    {
        get { return Time.unscaledDeltaTime; }
    }

    void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _snapping = false;
            _previousSwipePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.y != _previousSwipePosition.y)
            {
                var moveAmount = (_previousSwipePosition.y - Input.mousePosition.y) * ScrollSensitivity;
                moveAmount /= (float)Screen.height;
                _yPos += moveAmount;
                _previousSwipePosition = Input.mousePosition;
                ClampView();
                _velocity = GetVelocity();
            }
        }
        else if (_velocity != 0)
        {
            if (_yPos > TopClamp)
            {
                SnapToPoint(TopClamp);
            }
            else
            {
                if (!_snapping &&Mathf.Abs(_velocity) > SnapVelocity)
                {
                    DampenVelocity();
                }
                else
                {
                    _snapping = true;
                    SnapToPoint(FindSnapToPosition());
                }
            }
            ApplyVelocity();
        }
        _previousYPos = _yPos;
    }

    private void ClampView()
    {
        if (_yPos > TopClamp)
        {
            var offset = _yPos - TopClamp;
            //_yPos += offset;
            _yPos = _yPos - RubberDelta(offset, ViewSize);
            //_yPos = TopClamp;
        }
    }
    private float GetVelocity()
    {
        float newVelocity = (_yPos - _previousYPos) / deltaTime;
        return Mathf.Lerp(_velocity, newVelocity, deltaTime * 10);
    }

    private void DampenVelocity()
    {
        _velocity *= Mathf.Pow(DecelarationRate, deltaTime);
        if (Mathf.Abs(_velocity) < 1)
            _velocity = 0;
    }

    private void ApplyVelocity()
    {
        _yPos += _velocity * deltaTime;
    }

    private static float RubberDelta(float overStretching, float viewSize)
    {
        return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
    }

    private void SnapToPoint(float target)
    {
        float speed = _velocity;
        var position = Mathf.SmoothDamp(_yPos, target, ref speed, SnapSpeed, Mathf.Infinity, deltaTime);
        _velocity = speed;
    }

    private float FindSnapToPosition()
    {
        var layer = Mathf.RoundToInt((_yPos - TopClamp) / LayerHeight);
        return TopClamp - layer * -LayerHeight;
    }
}
