using UnityEngine;
using System.Collections.Generic;

public class AudioController : MonoBehaviour
{
    //public int NumberOfAudioSources;
    public List<AudioClip> ZombieDropSounds = new List<AudioClip>();

    //private AudioSource[] _audioSources;
    //private int _lastSourceUsed = 0;

    private void Start()
    {
        Gameboard.Instance.TileDestroyed += Instance_TileDestroyed;
    }

    private void Instance_TileDestroyed(GameTile sender)
    {
        if(sender.Width > 1 || sender.Height > 1)
        {
            PlayZomieSound();
        }
    }

    public void PlayZomieSound()
    {
        var clip = ZombieDropSounds[ Random.Range(0, ZombieDropSounds.Count)];
        PlayClip(clip);
    }

    public void PlayClip(AudioClip clip)
    {
    //    var index = _lastSourceUsed;
    //    if (index >= _audioSources.Length)
    //        index = 0;
    //    _audioSources[index].clip = clip;
        AudioSource.PlayClipAtPoint(clip, Vector3.zero, SaveData.SFXVolume / 5f);
    }
}
