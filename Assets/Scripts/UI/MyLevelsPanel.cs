using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class MyLevelsPanel : MonoBehaviour
{
    public LevelEntry LevelEntryPrefab;
    public RectTransform Content;
    public Color EvenEntryColor;
    public Color OddEntryColor;

    public void GoToMenu()
    {
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("GoToMenu");

    }

    public void NewLevel()
    {
        Gameboard.Instance.CurrentLevel = null;
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("CreateNewLevel");
    }

    void Awake()
    {
        WebManager.Instance.ObtainedMyLevels += Insstance_ObtainedMyLevels;
        WebManager.Instance.ObtainedLevel += Insstance_ObtainedLevel;
    }

    private void Insstance_ObtainedLevel(string name, byte[] data, int id)
    {
        var layout = BoardLayoutImporter.GetBoardLayoutFromBinary(data);
        layout.name = name;
        layout.ID = id;
        Gameboard.Instance.CurrentLevel = layout;
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("DownloadComplete");
        //BoardLayoutImporter.ImportBinary(data);
    }

    private void Insstance_ObtainedMyLevels(JSONArray data)
    {
        FillContent(data);
    }

    private void FillContent(JSONArray data)
    {
        ClearContent();
        for(int i = 0;i < data.Count; i++)
        {
            var entry = Instantiate(LevelEntryPrefab);
            entry.GetComponent<Image>().color = i % 2 == 0 ? EvenEntryColor : OddEntryColor;
            entry.transform.SetParent(Content.transform);
            entry.LevelName = data[i]["LevelName"].Value;
            entry.LevelID = data[i]["id"].AsInt;
            entry.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void ClearContent()
    {
        for(int i = 0;i < Content.childCount; i++)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
    }
}
