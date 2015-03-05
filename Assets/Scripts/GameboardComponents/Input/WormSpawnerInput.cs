using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class WormSpawnerInput : MonoBehaviour 
{
    public Token WormHeadToken;
    public Token WormBodyToken;
    public WormProperties WormProperties;
    public bool SpawnAtStart = false;
    public Point SpawnPosition;

    void Awake()
    {
        Gameboard.Instance.GameboardReset += Instance_GameStarted;
    }

    private void Instance_GameStarted()
    {
        if(SpawnAtStart)
        {
            CreateWorm(SpawnPosition);
        }
    }

    public void CreateWorm(Point point)
    {
        WormCreator.CreateWorm(WormHeadToken, WormBodyToken, point, WormProperties);
    }
}
