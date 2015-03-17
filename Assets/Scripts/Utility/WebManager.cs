using UnityEngine;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[ExecuteInEditMode]
public class WebManager : MonoBehaviour {

    public delegate void PlaythroughsEvent(JSONArray playthroughs);
    public event PlaythroughsEvent PlaythroughsDownloaded;
    public delegate void PlaythroughEvent(List<PlaythroughAction> actions);
    public event PlaythroughEvent PlaythroughActionsDownloaded;
    public delegate void PlaythroughAndLevelEvent(List<PlaythroughAction> actions, byte[] levelData);
    public event PlaythroughAndLevelEvent PlaythroughActionsAndLevelDownloaded;
    public delegate void PlaythroughSubmitedEvent(int playthroughID);
    public event PlaythroughSubmitedEvent PlaythroughSubmited;
    public delegate void WWWLoadedEvent();
    public event WWWLoadedEvent PlaythroughDeletionComplete;
    public delegate void KeyValidationEvent(bool keyValidated, string testerName);
    public event KeyValidationEvent KeyValidationComplete;
    public delegate void MyLevelsEvent(JSONArray data);
    public event MyLevelsEvent ObtainedMyLevels;
    public delegate void SaveCompleteEvent(string levelName, int id);
    public event SaveCompleteEvent SaveComplete;
    public delegate void ObtainedLevelEvent(string name, byte[] data, int id);
    public event ObtainedLevelEvent ObtainedLevel;
    public delegate void ObtainedAllLevelsEvent(JSONArray data);
    public event ObtainedAllLevelsEvent ObtainedAllLevels;

    WWW www;
    string SubmitPlaythroughURL = "http://pennyanfootballpool.com/MonsterMashup/SubmitPlaythrough.php";
    string AddFeedbackToPlaythroughURL = "http://pennyanfootballpool.com/MonsterMashup/AddFeedbackToPlaythrough.php";
    string GetPlaythroughsURL = "http://pennyanfootballpool.com/MonsterMashup/GetPlaythroughs.php";
    string GetPlaythroughActionsURL = "http://pennyanfootballpool.com/MonsterMashup/GetPlaythroughActions.php";
    string GetPlaythroughActionsAndLevelURL = "http://pennyanfootballpool.com/MonsterMashup/GetPlaythroughActionsAndLevel.php";
    string DeletePlaythroughURL = "http://pennyanfootballpool.com/MonsterMashup/DeletePlaythrough.php";
    string ValidateKeyURL = "http://pennyanfootballpool.com/MonsterMashup/ValidateKey.php";
    string SaveLevelURL = "http://pennyanfootballpool.com/MonsterMashup/SaveLevel.php";
    string GetMyLevelsURL = "http://pennyanfootballpool.com/MonsterMashup/GetMyLevels.php";
    string GetLevelURL = "http://pennyanfootballpool.com/MonsterMashup/GetLevel.php";
    string GetAllLevelsURL = "http://pennyanfootballpool.com/MonsterMashup/GetAllLevels.php";
    string SubmitLevelUR = "http://pennyanfootballpool.com/MonsterMashup/SubmitLevel.php";

    byte actionsExportVersion = 1;

    public float Progress { get { return www.progress; } }
    public static WebManager Instance { get { return _instance; } }
    private static WebManager _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

    public void PostPlaythrough(Playthrough playthrough)
    {

        WWWForm form = new WWWForm();
        form.AddField("LevelID", playthrough.LevelID);
        form.AddField("TesterName", playthrough.TesterName);
        form.AddField("TesterKey", playthrough.TesterKey);
        form.AddField("TotalMoves", playthrough.TotalMoves);
        form.AddField("MovesOnWin", playthrough.MovesOnWin);
        form.AddField("Difficulty", playthrough.DifficultyRating);
        form.AddField("Satisfaction", playthrough.SatisfactionRating);
        form.AddField("Notes", playthrough.Notes);
        form.AddField("Duration", playthrough.Duration.TotalSeconds.ToString());
        form.AddField("Actions", ExportActions(playthrough.Actions));

        www = new WWW(SubmitPlaythroughURL, form);
        StopAllCoroutines();
        StartCoroutine("PostPlaythroughRoutine");
        //yield web;
    }
    public void AddFeedbackToPlaythrough(int id, int difficulty, int satisfaction, string notes)
    {
        WWWForm form = new WWWForm();
        form.AddField("PlaythroughID", id);
        form.AddField("TesterKey", ValidateKey.Key);
        form.AddField("Difficulty", difficulty);
        form.AddField("Satisfaction", satisfaction);
        form.AddField("Notes", notes);

        www = new WWW(AddFeedbackToPlaythroughURL, form);
        StopAllCoroutines();
        StartCoroutine("AddFeedbackRoutine");
    }
    public void GetPlaythroughsForLevel(int levelID, string key, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("Key", key);
        form.AddField("Password", password);
        form.AddField("LevelID", levelID);

        www = new WWW(GetPlaythroughsURL, form);
        StopAllCoroutines();
        StartCoroutine("GetPlaythroughsRoutine");
    }
    public void GetPlaythroughActions(int playthroughID, string key, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("PlaythroughID", playthroughID);
        form.AddField("Key", key);
        form.AddField("Password", password);

        www = new WWW(GetPlaythroughActionsURL, form);
        StopAllCoroutines();
        StartCoroutine("GetPlaythroughActionsRoutine");
    }
    public void GetPlaythroughActionsAndLevel(int playthroughID, string key, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("PlaythroughID", playthroughID);
        form.AddField("Key", key);
        form.AddField("Password", password);

        www = new WWW(GetPlaythroughActionsAndLevelURL, form);
        StopAllCoroutines();
        StartCoroutine("GetPlaythroughActionsAndLevelRoutine");
    }
    public void DeletePlaythrough(Playthrough playthrough, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("PlaythroughID", playthrough.ID);
        form.AddField("Password", password);

        www = new WWW(DeletePlaythroughURL, form);
        StopAllCoroutines();
        StartCoroutine("DeletePlaythroughRoutine");
    }
    public void CheckKeyValidation(string key)
    {
        WWWForm form = new WWWForm();
        form.AddField("Key", key);

        www = new WWW(ValidateKeyURL, form);
        StopAllCoroutines();
        StartCoroutine("ValidateKeyRoutine");
    }
    public void SaveLevel(BoardLayout layout, string key = null)
    {
        SaveLevel(layout.name, BoardLayoutExporter.ExportBinary(layout), layout.ID, key);
    }
    public void SaveLevel(string levelName, byte[] data, int levelID = -1, string key = null)
    {
        if (RequestParameters.HasKey("key") || key != null)
        {
            WWWForm form = new WWWForm();
            form.AddField("LevelName", levelName);
            form.AddField("AuthorKey", key != null ? key : RequestParameters.GetValue("key"));
            form.AddField("LevelData", System.Convert.ToBase64String(data));
            form.AddField("LevelID", levelID);

            www = new WWW(SaveLevelURL, form);
            //StopAllCoroutines();
            StartCoroutine("SaveLevelRoutine");
        }
        else
        {
            Debug.LogError("Cannot submit level, no key is present.");
        }
    }
    public void GetMyLevels()
    {
        if (RequestParameters.HasKey("key"))
        {
            WWWForm form = new WWWForm();
            form.AddField("Key", RequestParameters.GetValue("key"));
            www = new WWW(GetMyLevelsURL, form);
            StopAllCoroutines();
            StartCoroutine("GetMyLevelsRoutine");
        }
        else
        {
            Debug.LogError("Cannot get levels, no key is present.");
        }
    }
    public void GetLevel(int levelID, string key)
    {
        WWWForm form = new WWWForm();
        form.AddField("Key", key);
        form.AddField("LevelID", levelID);
        www = new WWW(GetLevelURL, form);
        StopAllCoroutines();
        StartCoroutine("GetLevelRoutine");
    }
    public void GetAllLevels(string key, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("Key", key);
        form.AddField("Password", password);
        www = new WWW(GetAllLevelsURL, form);
        StopAllCoroutines();
        StartCoroutine("GetAllLevelsRoutine");
    }
    public void SubmitLevel(string levelName, byte[] levelData, Playthrough playthrough)
    {
        if (ValidateKey.Key.Length > 0)
        {
            WWWForm form = new WWWForm();
            AddPlaythroughDataToForm(playthrough, form);
            form.AddField("LevelName", levelName);
            form.AddField("LevelData", System.Convert.ToBase64String(levelData));

            www = new WWW(SubmitLevelUR, form);
            StopAllCoroutines();
            StartCoroutine("SubmitLevelRoutine");
        }
        else
        {
            Debug.LogError("No key");
        }
    }

    #region CoRoutines
    System.Collections.IEnumerator PostPlaythroughRoutine()
    {
        yield return www;
        PrintServerResponse(www);

        var json = JSON.Parse(www.text);
        int id = json["PlaythroughID"].AsInt;

        if (PlaythroughSubmited != null)
            PlaythroughSubmited(id);
    }
    System.Collections.IEnumerator AddFeedbackRoutine()
    {
        yield return www;
        PrintServerResponse(www);
    }
    System.Collections.IEnumerator GetPlaythroughsRoutine()
    {
        yield return www;
        PrintServerResponse(www);

        var json = JSON.Parse(www.text);
        //var playthroughs = ExtractPlaythroughsFromJSON(json);

        if (PlaythroughsDownloaded != null)
            PlaythroughsDownloaded(json["Playthroughs"].AsArray);
    }
    System.Collections.IEnumerator GetPlaythroughActionsRoutine()
    {
        yield return www;
        PrintServerResponse(www);

        var json = JSON.Parse(www.text);
        List<PlaythroughAction> actions = ImportActions(json["Actions"].Value);
        if (PlaythroughActionsDownloaded != null)
            PlaythroughActionsDownloaded(actions);
    }
    System.Collections.IEnumerator GetPlaythroughActionsAndLevelRoutine()
    {
        yield return www;
        PrintServerResponse(www);

        var json = JSON.Parse(www.text);
        byte[] data = System.Convert.FromBase64String(json["LevelData"].Value);
        List<PlaythroughAction> actions = ImportActions(json["Actions"].Value);
        if (PlaythroughActionsAndLevelDownloaded != null)
            PlaythroughActionsAndLevelDownloaded(actions, data);
    }
    System.Collections.IEnumerator DeletePlaythroughRoutine()
    {
        yield return www;
        PrintServerResponse(www);
        if (PlaythroughDeletionComplete != null)
            PlaythroughDeletionComplete();
    }
    System.Collections.IEnumerator ValidateKeyRoutine()
    {
        yield return www;
        PrintServerResponse(www);
        var json = JSON.Parse(www.text);
        bool validated = json["Validated"].AsBool;
        if(validated)
        Debug.Log(validated);
        if (KeyValidationComplete != null)
            KeyValidationComplete(json["Validated"].AsBool, json["Name"].Value);
    }
    System.Collections.IEnumerator SaveLevelRoutine()
    {
        yield return www;
        PrintServerResponse(www);
        var json = JSON.Parse(www.text);

        if (SaveComplete != null)
            SaveComplete(json["LevelInfo"]["LevelName"].Value, json["LevelInfo"]["ID"].AsInt);
        //Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("SaveComplete");
    }
    System.Collections.IEnumerator GetMyLevelsRoutine()
    {
        yield return www;
        PrintServerResponse(www);
        var json = JSON.Parse(www.text);
        Gameboard.Instance.GetComponent<PlayMakerFSM>().Fsm.Event("DownloadComplete");
        if (ObtainedMyLevels != null)
            ObtainedMyLevels(json["Levels"].AsArray);
    }
    System.Collections.IEnumerator GetLevelRoutine()
    {
        yield return www;
        PrintServerResponse(www);
        var json = JSON.Parse(www.text);
        bool levelFound = json["LevelFound"].AsBool;

        if (levelFound)
        {
            if (ObtainedLevel != null)
            {
                string name = json["Level"]["LevelName"].Value;
                byte[] data = System.Convert.FromBase64String(json["Level"]["LevelData"].Value);
                int id = json["Level"]["id"].AsInt;
                ObtainedLevel(name, data, id);
            }
        }
        else
        {
            Debug.LogError("Could not find level");
        }
    }
    System.Collections.IEnumerator GetAllLevelsRoutine()
    {
        yield return www;
        PrintServerResponse(www);
        var json = JSON.Parse(www.text);
        var levels = json["Levels"].AsArray;

        if (ObtainedAllLevels != null)
            ObtainedAllLevels(levels);
    }
    System.Collections.IEnumerator SubmitLevelRoutine()
    {
        yield return www;
        PrintServerResponse(www);
    }
    #endregion

    #region HelperMethods
    private void PrintServerResponse(WWW www)
    {
        Debug.Log(www.text);
        var json = JSON.Parse(www.text);

        string msg = json["Message"].Value;
        msg.Replace("\\n", "\n");
        msg.Replace("\\r", "\r");
        Debug.Log(msg);

    }
    private void AddPlaythroughDataToForm(Playthrough playthrough, WWWForm form)
    {
        form.AddField("LevelID", playthrough.LevelID);
        form.AddField("TesterName", playthrough.TesterName);
        form.AddField("TesterKey", playthrough.TesterKey);
        form.AddField("TotalMoves", playthrough.TotalMoves);
        form.AddField("MovesOnWin", playthrough.MovesOnWin);
        form.AddField("Difficulty", playthrough.DifficultyRating);
        form.AddField("Satisfaction", playthrough.SatisfactionRating);
        form.AddField("Notes", playthrough.Notes);
        form.AddField("Duration", playthrough.Duration.TotalSeconds.ToString());
        form.AddField("Actions", ExportActions(playthrough.Actions));
    }
    string ExportActions(List<PlaythroughAction> actions)
    {
        string result = "";
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(actionsExportVersion);
                writer.Write(actions.Count);
                foreach (var action in actions)
                {
                    if (action is InputAction)
                    {
                        writer.Write((byte)0);
                        writer.Write((byte)((InputAction)action).Direction);
                        writer.Write(action.Time);
                        writer.Write(((InputAction)action).InputValidated);
                    }
                    else if (action is RetryAction)
                    {
                        writer.Write((byte)1);
                        writer.Write(action.Time);
                    }
                }

                writer.Close();
                ms.Close();
                result = System.Convert.ToBase64String(ms.ToArray());
            }
        }
        return result;
    }
    List<PlaythroughAction> ImportActions(string data)
    {
        List<PlaythroughAction> results = new List<PlaythroughAction>();
        byte[] bytes = Convert.FromBase64String(data);
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int exportVersion = reader.ReadByte();
                int numOfActions = reader.ReadInt32();
                for(int i = 0;i < numOfActions; i++)
                {
                    byte actionType = reader.ReadByte();
                    switch(actionType)
                    {
                        case 0:
                            if(exportVersion == 0)
                                results.Add(new InputAction((Direction)reader.ReadByte(), reader.ReadSingle(), true));
                            else if(exportVersion == 1)
                                results.Add(new InputAction((Direction)reader.ReadByte(), reader.ReadSingle(), reader.ReadBoolean()));

                            break;
                        case 1:
                            results.Add(new RetryAction(reader.ReadSingle()));
                            break;
                    }
                }
                //while()
            }
        }
        return results;
    }
    private List<Playthrough> ExtractPlaythroughsFromJSON(JSONNode json)
    {
        List<Playthrough> result = new List<Playthrough>();

        var playthroughs = json["Playthroughs"].AsArray;
        for(int i = 0;i < playthroughs.Count; i++)
        {
            var p = playthroughs[i];
            result.Add(new Playthrough
            {
                ID = p["id"].AsInt,
                LevelID = p["LevelID"].AsInt,
                TesterName = p["TesterName"].Value,
                TotalMoves = p["TotalMoves"].AsInt,
                MovesOnWin = p["MovesOnWin"].AsInt,
                DifficultyRating = p["Difficulty"].AsInt,
                SatisfactionRating = p["Satisfaction"].AsInt,
                Notes = p["Notes"].Value,
                Duration = System.TimeSpan.FromSeconds(p["Duration"].AsFloat),
                DateTime = UnixTimeStampToDateTime(p["Time"].AsInt)
            });
        }
        return result;
    }
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
    #endregion
}
