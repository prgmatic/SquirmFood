using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class SwipeInput : MonoBehaviour
{


    //private bool swiping = false;
    //private bool eventSent = false;
    //private Vector2 lastPosition;

    private float minimumSwipeDistance = 0;
    private Vector2 _touchStartPosition = Vector2.zero;
    private bool _watchingForSwipe = false;

    void Awake()
    {
        var smallerDimension = Mathf.Min(Screen.width, Screen.height);
        minimumSwipeDistance = smallerDimension / 10;
        //Debug.Log(minimumSwipeDistance);
    }

    void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        var touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            _touchStartPosition = touch.position;
            _watchingForSwipe = true;
        }
        else if(_watchingForSwipe && touch.phase == TouchPhase.Ended)
        {
            _watchingForSwipe = false;
            Vector2 delta = touch.position - _touchStartPosition;
            var dot = Vector2.Dot(delta.normalized, Vector2.up);
            if(delta.magnitude >= minimumSwipeDistance)
            {
                if(dot > .7071f)
                {
                    Debug.Log("Up");
                    Swipe(Direction.Up);
                }
                else if(dot < -.7071f)
                {
                    Debug.Log("Down");
                    Swipe(Direction.Down);
                }
                else if(delta.x > 0)
                {
                    Debug.Log("Right");
                    Swipe(Direction.Right);
                }
                else if(delta.x < 0)
                {
                    Debug.Log("Left");
                    Swipe(Direction.Left);
                }
            }
        }
    }


    private void Swipe(Direction direction)
    {
        Gameboard.Instance.MoveWorm(direction);
    }
}
