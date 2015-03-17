using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField]
    private Image _loadingImage = null;

    void Update()
    {
        _loadingImage.transform.Rotate(0, 0, -5f * Time.deltaTime * 60);
    }
	
}
