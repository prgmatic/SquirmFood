using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class WormSpawnerInput : MonoBehaviour 
{
    public Token WormHeadToken;
    public Token WormBodyToken;
    public WormProperties WormProperties;

	void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            WormCreator.CreateWorm(WormHeadToken, WormBodyToken, Utils.CursorGridPosotion, WormProperties);
        }
    }
}
