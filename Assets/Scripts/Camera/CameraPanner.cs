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

    public float LayerHeight = 22f;
    public float ScrollSensitivity = 1f;
    public float DecelarationRate = 0.135f;
    public float Elasticity = .1f;
    public float ViewSize = 1.0f;
    public float SnapSpeed = .3f;
    public float SnapVelocity = 20f;

    public bool CanScroll
    {
        get { return GameManager.Instance.State != GameManager.GameState.PlayingGame; }
    }

    private float _maxScroll;
    private float _minScroll;

    private Vector2 _previousSwipePosition;
    private float _velocity = 0f;
    private float _previousYPos = 0f;
    private bool _snapping = false;
    private bool _panning = false;

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

    public void WarpCameraToStart()
    {
        if (!enabled)
            return;
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
        float yPos = FirstGameboardYPos - (LayerHeight * gameboardNumber);
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

    private void Start()
    {
        _maxScroll = GameStartYpos;
    }

    private void Update()
    {
        if (!CanScroll || _panning) return;
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
            if (_yPos > _maxScroll)
            {
                SnapToPoint(_maxScroll);
            }
            else
            {
                if (!_snapping && Mathf.Abs(_velocity) > SnapVelocity)
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

    private IEnumerator Pan(float yPos)
    {
        if (!enabled)
            yield break;
        _panning = true;
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
        _panning = false;
        if (PanComplete != null)
            PanComplete();
    }

    private void ClampView()
    {
        if (_yPos > _maxScroll)
        {
            var offset = _yPos - _maxScroll;
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
        if(_yPos > MonitorYPos)
        {
            float distFromStart = Mathf.Abs(GameStartYpos - _yPos);
            float distFromMonitor = Mathf.Abs(MonitorYPos - _yPos);

            if(distFromStart < distFromMonitor)
                return GameStartYpos;
            return MonitorYPos;
        }
        else if (_yPos > FirstGameboardYPos)
        {
            float distFromMonitor = Mathf.Abs(MonitorYPos - _yPos);
            float distFromFirst = Mathf.Abs(FirstGameboardYPos - _yPos);

            if (distFromMonitor < distFromFirst)
                return MonitorYPos;
            return FirstGameboardYPos;

            //return _yPos;
        }

        var layer = Mathf.RoundToInt((_yPos - FirstGameboardYPos) / LayerHeight);
        return FirstGameboardYPos - layer * -LayerHeight;
    }

}
