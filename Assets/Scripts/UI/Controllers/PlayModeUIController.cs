using UnityEngine;
using System.Collections.Generic;

public class PlayModeUIController : MonoBehaviour 
{
	private PlayMakerFSM fsm;

	void Awake()
	{
		fsm = GetComponent<PlayMakerFSM>();
		Gameboard.Instance.GameStarted += Instance_GameStarted;
		Gameboard.Instance.GameEnded += Instance_GameEnded;
	}

	private void Instance_GameEnded()
	{
		fsm.Fsm.Event("GameEnded");
	}

	private void Instance_GameStarted()
	{
		fsm.Fsm.Event("GameStarted");
	}
}
