using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public bool Auto = true;

    private bool _spawning = false;

    private void Update()
    {
        if (ObjectToSpawn == null)
            return;
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                _spawning = true;
            }
#else
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                _spawning = true;
            }
#endif

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _spawning = false;
        }

        if (_spawning)
        {
            Instantiate(ObjectToSpawn, Utils.CursorPositionInWorld, Quaternion.identity);
            if (!Auto) _spawning = false;
        }
    }
}
