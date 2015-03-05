using UnityEngine;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using System;
using System.Runtime.Serialization.Formatters.Binary;


public class PlaythroughDatabase : MonoBehaviour {

    public delegate void PlaythroughsEvent(List<Playthrough> playthroughs);
    public event PlaythroughsEvent PlaythroughsDownloaded;
    public delegate void PlaythroughEvent(Playthrough playthrough);
    public event PlaythroughEvent PlaythroughActionsDownloaded;
    public delegate void PlaythroughSubmitedEvent(int playthroughID);
    public event PlaythroughSubmitedEvent PlaythroughSubmited;
    public delegate void WWWLoadedEvent();
    public event WWWLoadedEvent PlaythroughDeletionComplete;
    public delegate void KeyValidationEvent(bool keyValidated, string testerName);
    public event KeyValidationEvent KeyValidationComplete;

    WWW www;
    string SubmitPlaythroughURL = "http://pennyanfootballpool.com/MonsterMashup/SubmitPlaythrough.php";
    string AddFeedbackToPlaythroughURL = "http://pennyanfootballpool.com/MonsterMashup/AddFeedbackToPlaythrough.php";
    string GetPlaythroughsURL = "http://pennyanfootballpool.com/MonsterMashup/GetPlaythroughs.php";
    string GetPlaythroughActionsURL = "http://pennyanfootballpool.com/MonsterMashup/GetPlaythroughActions.php";
    string DeletePlaythroughURL = "http://pennyanfootballpool.com/MonsterMashup/DeletePlaythrough.php";
    string ValidateKeyURL = "http://pennyanfootballpool.com/MonsterMashup/ValidateKey.php";

    byte actionsExportVersion = 1;

    public static PlaythroughDatabase Insstance { get { return _instance; } }
    private static PlaythroughDatabase _instance;

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
        form.AddField("TesterName", playthrough.TesterName);
        form.AddField("SceneGUID", playthrough.SceneGUID);
        form.AddField("LayoutGUID", playthrough.LayoutGUID);
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
        form.AddField("Difficulty", difficulty);
        form.AddField("Satisfaction", satisfaction);
        form.AddField("Notes", notes);

        www = new WWW(AddFeedbackToPlaythroughURL, form);
        StopAllCoroutines();
        StartCoroutine("AddFeedbackRoutine");
    }
    public void GetPlaythroughsForLayout(string sceneGUID, string layoutGUID, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("SceneGUID", sceneGUID);
        form.AddField("LayoutGUID", layoutGUID);
        form.AddField("Password", password);

        www = new WWW(GetPlaythroughsURL, form);
        StopAllCoroutines();
        StartCoroutine("GetPlaythroughsRoutine");
    }
    public void GetPlaythroughActions(Playthrough playthrough, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("PlaythroughID", playthrough.ID);
        form.AddField("Password", password);

        www = new WWW(GetPlaythroughActionsURL, form);
        StopAllCoroutines();
        StartCoroutine("GetPlaythroughActionsRoutine", playthrough);
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
    public void ValidateKey(string key)
    {
        WWWForm form = new WWWForm();
        form.AddField("Key", key);

        www = new WWW(ValidateKeyURL, form);
        StopAllCoroutines();
        StartCoroutine("ValidateKeyRoutine");
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
        var playthroughs = ExtractPlaythroughsFromJSON(json);

        if (PlaythroughsDownloaded != null)
            PlaythroughsDownloaded(playthroughs);
    }
    System.Collections.IEnumerator GetPlaythroughActionsRoutine(Playthrough playthrough)
    {
        yield return www;
        PrintServerResponse(www);

        var json = JSON.Parse(www.text);
        playthrough.Actions = ImportActions(json["Actions"].Value);
        if (PlaythroughActionsDownloaded != null)
            PlaythroughActionsDownloaded(playthrough);
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
                TesterName = p["TesterName"],
                SceneGUID = p["SceneGUID"],
                LayoutGUID = p["LayoutGUID"],
                TotalMoves = p["TotalMoves"].AsInt,
                MovesOnWin = p["MovesOnWin"].AsInt,
                DifficultyRating = p["Difficulty"].AsInt,
                SatisfactionRating = p["Satisfaction"].AsInt,
                Notes = p["Notes"],
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
