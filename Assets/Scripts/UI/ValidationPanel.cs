using UnityEngine;
using System.Collections;

public class ValidationPanel : MonoBehaviour {


    public static ValidationPanel Instance { get { return _instance; } }
    private static ValidationPanel _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (this != _instance)
            Destroy(this.gameObject);
        Hide();
    }

    public void Show()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void Hide()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Test()
    {
        Debug.Log("This is a test!");
    }
}
