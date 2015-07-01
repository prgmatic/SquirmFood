using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SimpleJSON;

public class PlaythroughBrowser : EditorWindow
{
    const string SlothPunchKey = "54fee90bed334";
    const string PWord = "GoTeam";
    List<DatabaseLevelEntry> levels = new List<DatabaseLevelEntry>();
    List<DatabasePlaythroughEntry> playthroughs = new List<DatabasePlaythroughEntry>();
    private DatabaseLevelEntry selectedLevel = null;
    Vector2 scrollPosition;
    private BrowsingState browsingState = BrowsingState.WaitingToGetLevels;
    private PlaythroughViewer.PlaybackType _playbackType = PlaythroughViewer.PlaybackType.Realtime;

    private Sprite silverStar;
    private Sprite goldStar;
    private GUIStyle lightStyle;
    private GUIStyle darkStyle;

    const float NAME_WIDTH = 150f;
    const float VERSION_WIDTH = 50f;
    const float NOP_WIDTH = 30f;
    const float AVG_D_WIDTH = 40f;
    const float SOLVE_TIME_WIDTH = 50f;
    const float DATE_WDITH = 150f;
    const float RATING_WIDTH = 115f;
    const float IP_WIDTH = 90f;
    const float PLAYTHROUGHS_BUTTON_WIDTH = 100f;

    //[MenuItem("Worm Food/Playthrough Browser", false, 1)]
    //public static void OpenBrowser()
    //{
    //    EditorWindow.GetWindow(typeof(PlaythroughBrowser));
    //}

    #region Initializers
    void OnEnable()
    {
        this.title = "Levels";
        EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
    }
    void Init()
    {
        LoadTextures();
        LoadStyles();
    }
    private void LoadTextures()
    {
        silverStar = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/SilverStar.png", typeof(Sprite));
        goldStar = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/GoldStar.png", typeof(Sprite));

    }
    private void LoadStyles()
    {
        darkStyle = new GUIStyle();
        darkStyle.normal.background = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        darkStyle.normal.background.SetPixel(0, 0, new Color(0.78f, 0.78f, 0.78f));
        darkStyle.normal.background.Apply();

        lightStyle = new GUIStyle();
        lightStyle.normal.background = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        lightStyle.normal.background.SetPixel(0, 0, new Color(0.81f, 0.81f, 0.81f));
        lightStyle.normal.background.Apply();
    }
    #endregion

    void OnGUI()
    {
        if (Application.isPlaying)
        {
            switch(browsingState)
            {
                case BrowsingState.WaitingToGetLevels:
                    DrawGetLevelsButton();
                    break;
                case BrowsingState.BrowsingLevels:
                    DrawLevelEntries();
                    break;
                case BrowsingState.DownloadingLevels:
                    DrawLoading();
                    break;
                case BrowsingState.BrowsingPlaythroughs:
                    DrawPlaythroughEntries();
                    break;
                case BrowsingState.DownlaodingPlaythroughs:
                    DrawLoading();
                    break;
                case BrowsingState.DownlaodingLevelData:
                    DrawLoading();
                    break;
            }
        }
        else
            DrawAtCenter("You must be in play mode to use the playthrough browser.");
    }
    #region HelperMethods
    private void GetAllLevels()
    {
        levels.Clear();
        scrollPosition = Vector2.zero;
        browsingState = BrowsingState.DownloadingLevels;
        WebManager.Instance.ObtainedAllLevels += Insstance_ObtainedAllLevels;
        WebManager.Instance.GetAllLevels(SlothPunchKey, PWord);
    }
    private void GetPlaythroughsForLevel(DatabaseLevelEntry level)
    {
        playthroughs.Clear();
        selectedLevel = level;
        scrollPosition = Vector2.zero;
        browsingState = BrowsingState.DownlaodingPlaythroughs;
        WebManager.Instance.PlaythroughsDownloaded += Insstance_PlaythroughsDownloaded;
        WebManager.Instance.GetPlaythroughsForLevel(level.ID, SlothPunchKey, PWord);
    }
    private void DownloadLevel(DatabaseLevelEntry level)
    {
        browsingState = BrowsingState.DownlaodingLevelData;
        WebManager.Instance.ObtainedLevel += Insstance_ObtainedLevel;
        WebManager.Instance.GetLevel(level.ID, SlothPunchKey);
    }

    


    #endregion

    #region DrawMethods
    private void DrawLoading()
    {
        DrawAtCenter("Loading...");
    }
    private void DrawAtCenter(string msg, GUIStyle style = null)
    {
        Rect pos = this.position;
        pos.x = 0; pos.y = 0;
        GUILayout.BeginArea(pos);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (style != null)
            GUILayout.Label(msg, style);
        else
            GUILayout.Label(msg);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
    private void DrawGetLevelsButton()
    {
        Rect pos = this.position;
        pos.x = 0; pos.y = 0;
        GUILayout.BeginArea(pos);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Get Levels"))
        {
            GetAllLevels();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
    private void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        if(browsingState == BrowsingState.BrowsingPlaythroughs && selectedLevel != null)
        {
            GUILayout.Label(selectedLevel.Name);
        }
        GUILayout.FlexibleSpace();
        if(browsingState == BrowsingState.BrowsingPlaythroughs)
        {
            if (GUILayout.Button("Back", EditorStyles.toolbarButton))
            {
                selectedLevel = null;
                GetAllLevels();
            }
        }
        if (GUILayout.Button("Refesh", EditorStyles.toolbarButton))
        {
            if (browsingState == BrowsingState.BrowsingLevels)
                GetAllLevels();
            else if (browsingState == BrowsingState.BrowsingPlaythroughs)
                GetPlaythroughsForLevel(selectedLevel);
        }
        GUILayout.EndHorizontal();
    }
    private void DrawLevelEntries()
    {
        DrawToolbar();
        DrawLevelHeader();
        if(levels.Count == 0)
        {
            DrawAtCenter("No level entries abailable, try refreshing.");
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for(int i = 0; i < levels.Count; i++)
        {
            DrawLevelEntry(levels[i], i % 2 == 0 ? lightStyle : darkStyle);
        }
        GUILayout.EndScrollView();
    }
    private void DrawLevelHeader()
    {
        TextAnchor alignmentBackup = EditorStyles.toolbarButton.alignment;
        EditorStyles.toolbarButton.alignment = TextAnchor.MiddleLeft;
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        DrawHeaderColumn("Name", NAME_WIDTH);
        DrawHeaderColumn("Author Name", NAME_WIDTH);
        DrawHeaderColumn("Version", VERSION_WIDTH);
        DrawHeaderColumn("NoP", NOP_WIDTH);
        DrawHeaderColumn("AvgD", AVG_D_WIDTH);
        DrawHeaderColumn("AvgS", AVG_D_WIDTH);
        DrawHeaderColumn("AvgST", SOLVE_TIME_WIDTH);
        DrawHeaderColumn("Date Created", DATE_WDITH);
        DrawHeaderColumn("Date Updated", DATE_WDITH);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorStyles.toolbarButton.alignment = alignmentBackup;
    }
    private void DrawLevelEntry(DatabaseLevelEntry level, GUIStyle style)
    {
        TextAnchor alignmentBackup = EditorStyles.label.alignment;
        GUIStyle lStyle = EditorStyles.label;
        GUILayout.BeginHorizontal(style);
        GUILayout.Space(8);
        DrawLabel(level.Name, NAME_WIDTH);
        DrawLabel(level.AuthorName, NAME_WIDTH);
        EditorStyles.label.alignment = TextAnchor.UpperRight;
        DrawLabel(level.Version.ToString(), VERSION_WIDTH);
        DrawLabel(level.NumberOfPlaythroughs.ToString(), NOP_WIDTH);
        DrawLabel(level.AvgD, AVG_D_WIDTH);
        DrawLabel(level.AvgS, AVG_D_WIDTH);
        DrawLabel(level.AvgST, SOLVE_TIME_WIDTH);
        DrawLabel(level.DateCreated, DATE_WDITH);
        DrawLabel(level.DateUpdated, DATE_WDITH);
        GUILayout.FlexibleSpace();

        if (level.NumberOfPlaythroughs > 0 && GUILayout.Button("Playthroughs", GUILayout.Width(PLAYTHROUGHS_BUTTON_WIDTH)))
        {
            GetPlaythroughsForLevel(level);
        }
        if(GUILayout.Button("Download", GUILayout.Width(PLAYTHROUGHS_BUTTON_WIDTH)))
        {
            DownloadLevel(level);
        }
        GUILayout.EndHorizontal();
        EditorStyles.label.alignment = alignmentBackup;
    }
    private void DrawPlaythroughEntries()
    {
        DrawToolbar();
        DrawPlaythroughHeader();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < playthroughs.Count; i++)
        {
            DrawPlaythroghEntry(playthroughs[i], i % 2 == 0 ? lightStyle : darkStyle);
        }
        GUILayout.EndScrollView();
    }
    private void DrawPlaythroughHeader()
    {
        TextAnchor alignmentBackup = EditorStyles.toolbarButton.alignment;
        EditorStyles.toolbarButton.alignment = TextAnchor.MiddleLeft;
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        DrawHeaderColumn("Tester Name", NAME_WIDTH);
        DrawHeaderColumn("IP Address", IP_WIDTH);
        DrawHeaderColumn("TM", AVG_D_WIDTH);
        DrawHeaderColumn("MoW", AVG_D_WIDTH);
        DrawHeaderColumn("Difficulty", RATING_WIDTH);
        DrawHeaderColumn("Satisfaction", RATING_WIDTH);
        DrawHeaderColumn("Duration", SOLVE_TIME_WIDTH);
        DrawHeaderColumn("Date", NAME_WIDTH);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorStyles.toolbarButton.alignment = alignmentBackup;
    }
    private void DrawPlaythroghEntry(DatabasePlaythroughEntry playthrough, GUIStyle style)
    {
        TextAnchor alignmentBackup = EditorStyles.label.alignment;
        GUIStyle lStyle = EditorStyles.label;
        GUILayout.BeginHorizontal(style);
        GUILayout.Space(8);
        DrawLabel(playthrough.TesterName, NAME_WIDTH);
        DrawLabel(playthrough.IP, IP_WIDTH);
        EditorStyles.label.alignment = TextAnchor.UpperRight;
        DrawLabel(playthrough.TotalMoves.ToString(), AVG_D_WIDTH);
        DrawLabel(playthrough.MovesOnWin.ToString(), AVG_D_WIDTH);
        DrawRating(playthrough.Difficulty, RATING_WIDTH);
        DrawRating(playthrough.Satisfaction, RATING_WIDTH);
        DrawLabel(playthrough.DurationString, SOLVE_TIME_WIDTH);
        DrawLabel(playthrough.DateTimeString, NAME_WIDTH);
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("View Replay"))
        {
            _playbackType = PlaythroughViewer.PlaybackType.Realtime;
            WebManager.Instance.PlaythroughActionsAndLevelDownloaded += Instance_PlaythroughActionsAndLevelDownloaded;
            WebManager.Instance.GetPlaythroughActionsAndLevel(playthrough.ID, SlothPunchKey, PWord);
            //Gameboard.Instance.GetComponent<PlaythroughViewer>().ViewPlaythrough()
        }
        if(GUILayout.Button("View Stepped"))
        {
            Debug.Log("View Stepped");
        }
        GUILayout.EndHorizontal();
        EditorStyles.label.alignment = alignmentBackup;
    }

    private void Instance_PlaythroughActionsAndLevelDownloaded(List<PlaythroughAction> actions, byte[] levelData)
    {
        WebManager.Instance.PlaythroughActionsAndLevelDownloaded -= Instance_PlaythroughActionsAndLevelDownloaded;
        Gameboard.Instance.GetComponent<PlaythroughViewer>().ViewPlaythrough(actions, BoardLayoutImporter.GetBoardLayoutFromBinary(levelData), _playbackType);
        Debug.Log("got it");
    }

    private void DrawHeaderColumn(string content, float width)
    {
        GUILayout.Label(content, EditorStyles.toolbarButton, GUILayout.Width(width));

    }
    private void DrawLabel(string content, float width)
    {
        GUILayout.Label(content, EditorStyles.label, GUILayout.Width(width - 4));
    }
    private void DrawRating(int rating, float width)
    {
        float starSize = 16;
        if (rating > 0)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(width - 4));
            for (int i = 0; i < 5; i++)
            {
                if (i + 1 <= rating)
                    GUILayout.Label(goldStar.texture, GUILayout.Width(starSize), GUILayout.Height(starSize));
                else
                    GUILayout.Label(silverStar.texture, GUILayout.Width(starSize), GUILayout.Height(starSize));
            }
            GUILayout.EndHorizontal();
        }
        else DrawLabel("N/A", RATING_WIDTH);
    }
    #endregion

    #region Delegates
    private void PlaymodeStateChanged()
    {
        Init();
        browsingState = BrowsingState.WaitingToGetLevels;
        Repaint();
    }
    private void Insstance_ObtainedAllLevels(SimpleJSON.JSONArray data)
    {
        browsingState = BrowsingState.BrowsingLevels;
        WebManager.Instance.ObtainedAllLevels -= Insstance_ObtainedAllLevels;
        for(int i = 0;i < data.Count; i++)
        {
            levels.Add(new DatabaseLevelEntry(data[i].AsObject));
        }
        Repaint();
    }
    private void Insstance_PlaythroughsDownloaded(JSONArray data)
    {
        browsingState = BrowsingState.BrowsingPlaythroughs;
        WebManager.Instance.PlaythroughsDownloaded -= Insstance_PlaythroughsDownloaded;
        for(int i = 0; i < data.Count; i++)
        {
            playthroughs.Add(new DatabasePlaythroughEntry(data[i].AsObject));
        }
        Repaint();
    }
    private void Insstance_ObtainedLevel(string name, byte[] data, int id)
    {
        WebManager.Instance.ObtainedLevel -= Insstance_ObtainedLevel;
        //BoardLayoutExporterEditor.SaveLevel(BoardLayoutImporter.GetBoardLayoutFromBinary(data), name);
        browsingState = BrowsingState.BrowsingLevels;
        Repaint();
    }
    #endregion

    private enum BrowsingState
    {
        WaitingToGetLevels,
        BrowsingLevels,
        DownloadingLevels,
        BrowsingPlaythroughs,
        DownlaodingPlaythroughs,
        DownlaodingLevelData
    }
    private class DatabaseLevelEntry
    {
        #region Properties
        public string AvgD { get { return NullableFloatToString(AverageDifficulty, 2); } }
        public string AvgS { get { return NullableFloatToString(AverageSatisfaction, 2); } }
        public string AvgST {
            get
            {
                string rValue = NullableFloatToString(AverageSolveTime, 2);
                if (rValue != "N/A") rValue += "s";
                return rValue;
            }
        }
        public string DateCreated { get { return NullableDateTimeToString(TimeCreated); } }
        public string DateUpdated { get { return NullableDateTimeToString(TimeUpdated); } }
        #endregion

        #region Variables
        public int ID = -1;
        public string Name = "";
        public string AuthorName = "";
        public string AuthorKey = "";
        public int Version = 0;
        public System.DateTime? TimeCreated = null;
        public System.DateTime? TimeUpdated = null;
        public int NumberOfPlaythroughs = 0;
        public bool IsSubmission = false;
        public bool Public = false;
        public float? AverageDifficulty = null;
        public float? AverageSatisfaction = null;
        public float? AverageSolveTime = null;
        #endregion

        public DatabaseLevelEntry(JSONNode json)
        {
            ID = json["id"].AsInt;
            Name = json["LevelName"].Value;
            AuthorName = json["AuthorName"].Value;
            AuthorKey = json["AuthorKey"].Value;
            Version = json["Version"].AsInt;
            int unixTime = json["TimeCreated"].AsInt;
            if (unixTime > -1)
                TimeCreated = WebManager.UnixTimeStampToDateTime(unixTime);
            unixTime = json["TimeUpdated"].AsInt;
            if (unixTime > -1)
                TimeUpdated = WebManager.UnixTimeStampToDateTime(unixTime);
            NumberOfPlaythroughs = json["Playthroughs"].AsInt;
            IsSubmission = json["IsSubmission"].AsBool;
            Public = json["Public"].AsBool;
            AverageDifficulty = GetNullableFloatValue(json["AverageDifficulty"]);
            AverageSatisfaction = GetNullableFloatValue(json["AverageSatisfaction"]);
            AverageSolveTime = GetNullableFloatValue(json["AverageSolveTime"]);
        }

        private float? GetNullableFloatValue(JSONNode node)
        {
            if (node.Value.ToLower() == "null") return null;
            else return node.AsFloat;
        }
        private string NullableFloatToString(float? f, int? roundDigits = null)
        {
            if (f == null) return "N/A";
            else
            {
                if (roundDigits != null)
                {
                    return System.Math.Round(f.Value, roundDigits.Value).ToString();
                }
                else return f.Value.ToString();
            }
        }
        private string NullableDateTimeToString(System.DateTime? dateTime)
        {
            if (dateTime == null) return "N/A";
            else return string.Format("{0} {1}", dateTime.Value.ToShortTimeString(), dateTime.Value.ToShortDateString());
        }
    }
    private class DatabasePlaythroughEntry
    {
        public string DateTimeString { get { return DateTimeToString(DateTime); } }
        public string DurationString { get { return System.Math.Round(Duration, 2).ToString(); } }
        #region Variables
        public int ID = -1;
        public int Version = -1;
        public string TesterName = "";
        public string IP = "";
        public int TotalMoves = 0;
        public int MovesOnWin = 0;
        public int Difficulty = 0;
        public int Satisfaction = 0;
        public string Notes = "";
        public float Duration = 0f;
        public System.DateTime DateTime;
        #endregion

        public DatabasePlaythroughEntry(JSONNode json)
        {
            ID = json["id"].AsInt;
            Version = json["Version"].AsInt;
            TesterName = json["TesterName"].Value;
            IP = json["IP"];
            TotalMoves = json["TotalMoves"].AsInt;
            MovesOnWin = json["MovesOnWin"].AsInt;
            Difficulty = json["Difficulty"].AsInt;
            Satisfaction = json["Satisfaction"].AsInt;
            Notes = json["Notes"].Value;
            Duration = json["Duration"].AsFloat;
            DateTime = WebManager.UnixTimeStampToDateTime(json["Time"].AsInt);
        }

        private string DateTimeToString(System.DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToShortTimeString(), dateTime.ToShortDateString());
        }
    }
}
