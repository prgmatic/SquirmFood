using UnityEngine;
using System.Collections;

public class FrameRateSetter : MonoBehaviour {


	void Awake()
	{
		Application.targetFrameRate = 60;
	}
}
