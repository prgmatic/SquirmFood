using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlaythroughAction
{
    public float Time;
}

[System.Serializable]
public class InputAction : PlaythroughAction
{
    [SerializeField]
    public Direction Direction;
    public bool InputValidated = true;
    public InputAction(Direction direction, float time, bool inputValidated)
    {
        this.Direction = direction;
        this.Time = time;
        this.InputValidated = inputValidated;
    }
}

[System.Serializable]
public class RetryAction : PlaythroughAction
{
    public RetryAction(float time)
    {
        this.Time = time;
    }
}
