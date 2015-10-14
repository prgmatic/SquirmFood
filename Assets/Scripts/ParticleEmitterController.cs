using UnityEngine;
using System.Collections;

public class ParticleEmitterController : MonoBehaviour
{
    public int NumberOfParticles = 5;
    public bool EmitterIsChild = false;
    private ParticleSystem _emitter;

    void Awake()
    {
        if (EmitterIsChild)
            _emitter = GetComponentInChildren<ParticleSystem>();
        else
            _emitter = GetComponent<ParticleSystem>();

        if (_emitter == null)
            this.enabled = false;
    }

    public void Emit()
    {
        if (_emitter == null)
            Debug.LogError("shit");
        _emitter.Emit(NumberOfParticles);
    }
}
