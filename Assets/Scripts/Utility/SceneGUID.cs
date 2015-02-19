using UnityEngine;
using System.Collections;

public class SceneGUID : MonoBehaviour
{
    public static SceneGUID Instance { get { return _instance; } }
    private static SceneGUID _instance;

    public string GUID = "";

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

}
