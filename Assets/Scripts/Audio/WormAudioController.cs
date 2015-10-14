using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WormAudioController : MonoBehaviour
{
    public AudioClip WormMove;
    public AudioClip WormEat;
    public AudioClip TokenSlide;

    private AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayMoveSound()
    {
        _audio.clip = WormMove;
        _audio.Play();
    }

    public void PlayEatSound()
    {
        _audio.clip = WormEat;
        _audio.Play();
    }

    public void PlayTokenSlideSound()
    {
        _audio.clip = TokenSlide;
        _audio.Play();
    }
}
