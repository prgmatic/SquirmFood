using UnityEngine;
using System.Collections;

public class WormAnimationSettings : MonoBehaviour {

    Animator _animator;

    public float FrameRate = 15;
    public float MoveTime = .2f;
	
	void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.speed = FrameRate / 15f;
    }
	
}
