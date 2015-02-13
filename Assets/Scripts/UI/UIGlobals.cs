using UnityEngine;
using System.Collections;

public class UIGlobals : MonoBehaviour 
{
    public static UIGlobals Instance { get { return _instance; } }
    private static UIGlobals _instance;

    public GameOverPanel gameOverPanel;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (this != _instance)
            Destroy(this.gameObject);
    }
}
