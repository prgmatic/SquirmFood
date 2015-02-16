using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class MoveWormInput : MonoBehaviour 
{
    public KeyCode Up = KeyCode.UpArrow;
    public KeyCode Down = KeyCode.DownArrow;
    public KeyCode Left = KeyCode.LeftArrow;
    public KeyCode Right = KeyCode.RightArrow;

    void OnEnable()
    {
        FlickGesture flick = GetComponent<FlickGesture>();
        if(flick != null)
            flick.Flicked += Flick_Flicked;
    }

    private void Flick_Flicked(object sender, System.EventArgs e)
    {
        if (!Gameboard.Instance.AcceptingInput) return;
        FlickGesture flick = (FlickGesture)sender;

        Vector2 flickDirection = flick.ScreenFlickVector.normalized;
        flickDirection = SnapTo(flickDirection, 90);
        Debug.Log(flickDirection);

        Move(Mathf.RoundToInt(flickDirection.x), Mathf.RoundToInt(-flickDirection.y));

    }

	void Update()
    {
        if (!Gameboard.Instance.AcceptingInput) return;
        if (Input.GetKeyDown(Up)) Move(0, -1);
        if (Input.GetKeyDown(Down)) Move(0, 1);
        if (Input.GetKeyDown(Left)) Move(-1, 0);
        if (Input.GetKeyDown(Right)) Move(1, 0);
    }

    public void Move(int x, int y)
    {
        for (int i = 0; i < Gameboard.Instance.gameTiles.Count; i++)
        {
            GameTile tile = Gameboard.Instance.gameTiles[i];
            Worm worm = tile.GetComponent<Worm>();
            if (worm != null)
            {
                worm.Move(worm.Head.GridPosition.x + x, worm.Head.GridPosition.y + y);
            }
        }
    }

    Vector3 SnapTo(Vector3 v3, float snapAngle)
    {
        float angle = Vector3.Angle(v3, Vector3.up);
        if (angle < snapAngle / 2.0f)          // Cannot do cross product 
            return Vector3.up * v3.magnitude;  //   with angles 0 & 180
        if (angle > 180.0f - snapAngle / 2.0f)
            return Vector3.down * v3.magnitude;

        float t = Mathf.Round(angle / snapAngle);

        float deltaAngle = (t * snapAngle) - angle;

        Vector3 axis = Vector3.Cross(Vector3.up, v3);
        Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);
        return q * v3;
    }
}
