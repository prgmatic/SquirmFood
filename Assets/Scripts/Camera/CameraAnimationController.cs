using UnityEngine;
using System.Collections;

public class CameraAnimationController : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartAnim()
    {
        _animator.SetTrigger("Start");
    }
}
