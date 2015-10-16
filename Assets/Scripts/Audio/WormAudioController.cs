using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WormAudioController : MonoBehaviour
{
    private AudioSource _move;
    private AudioSource _eat;
    private AudioSource _slide;

    void Awake()
    {
        var audioSources = GetComponents<AudioSource>();
        _move = audioSources[0];
        _eat = audioSources[1];
        _slide = audioSources[2];
    }

    public void PlayMoveSound()
    {
        _move.Play();
    }

    public void PlayEatSound()
    {
        _eat.Play();
    }

    public void PlayTokenSlideSound()
    {
        _slide.Play();
    }

    private void Update()
    {
        if (GameManager.Instance != null)
            _move.volume = _eat.volume = _slide.volume = SaveData.FXVolume;
    }
}
