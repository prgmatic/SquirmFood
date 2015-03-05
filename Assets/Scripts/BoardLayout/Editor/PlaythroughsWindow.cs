using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlaythroughsWindow : EditorWindow
{
    private BoardLayout currentLayout;
    private Sprite silverStar;
    private Sprite goldStar;
    private GUIStyle lightStyle;
    private GUIStyle darkStyle;
    private string pWord = "GoTeam";
    private bool loading = false;
    private bool steppedPlayback = false;

    private List<Playthrough> playthroughs = new List<Playthrough>();

    void OnEnable()
    {
        this.title = "Playthroughs";
        EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
    }

    private void PlaymodeStateChanged()
    {
        Repaint();
    }

    void InitTextures()
    {
        
        if (darkStyle == null || darkStyle.normal.background == null)
        {
            darkStyle = new GUIStyle();
            darkStyle.normal.background = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            darkStyle.normal.background.SetPixel(0, 0, new Color(0.67f, 0.67f, 0.67f));
            darkStyle.normal.background.Apply();
            //style.normal.background = bg;
        }
        if (lightStyle == null || lightStyle.normal.background == null)
        {
            lightStyle = new GUIStyle();
            lightStyle.normal.background = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            lightStyle.normal.background.SetPixel(0, 0, new Color(0.81f, 0.81f, 0.81f));
            lightStyle.normal.background.Apply();
            //style.normal.background = bg;
        }
        if (silverStar == null || goldStar == null)
        {
            silverStar = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/SilverStar.png", typeof(Sprite));
            goldStar = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/GoldStar.png", typeof(Sprite));
        }
    }

    public void SetLayout(BoardLayout layout)
    {
        currentLayout = layout;

        string sceneGUID = AssetDatabase.AssetPathToGUID(EditorApplication.currentScene);
        string layoutGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(layout));
        PlaythroughDatabase.Insstance.PlaythroughsDownloaded += Insstance_PlaythroughsDownloaded;
        PlaythroughDatabase.Insstance.GetPlaythroughsForLayout(sceneGUID, layoutGUID, pWord);
        loading = true;
    }

    

    void OnGUI()
    {
        if(!Application.isPlaying)
        {
            currentLayout = null;
            DrawAtCenter("Playthroughs can only be viewed when in play mode");
            return;
        }

        if(loading)
        {
            DrawAtCenter("Downloading Playthroughs...");
            return;
        }
        InitTextures();
        //style.normal.background = bg;
        if (currentLayout != null && currentLayout.Playthroughs.Count > 0)
        {
            for(int i = 0;i < currentLayout.Playthroughs.Count; i++)
            {
                

                Playthrough playthrough = currentLayout.Playthroughs[i];
                if(i % 2 == 1)
                    GUILayout.BeginHorizontal(new GUIContent(""), darkStyle);
                else
                    GUILayout.BeginHorizontal(new GUIContent(""), lightStyle);

                DrawNameAndDate(150, playthrough);
                DrawDurationMovesRetries(200, playthrough);
                DrawRatings(200, playthrough);
                GUILayout.FlexibleSpace();

                bool delete = DrawButtons(150, playthrough);
                GUILayout.EndHorizontal();

                if(delete)
                {
                    loading = true;
                    currentLayout.Playthroughs.Clear();
                    PlaythroughDatabase.Insstance.PlaythroughDeletionComplete += Insstance_PlaythroughDeletionComplete;
                    PlaythroughDatabase.Insstance.DeletePlaythrough(playthrough, pWord);
                    break;
                }
            }
        }
        else
        {
            if (currentLayout == null)
                DrawAtCenter("No scene selected");
            else
                DrawAtCenter("No playthroughs logged for this level in this scene");
        }
    }

    

    #region DrawMethods
    private void DrawAtCenter(string msg, GUIStyle style = null)
    {
        Rect pos = this.position;
        pos.x = 0; pos.y = 0;
        GUILayout.BeginArea(pos);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(style != null)
            GUILayout.Label(msg, style);
        else
            GUILayout.Label(msg);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
    private void DrawNameAndDate(float width, Playthrough playthrough)
    {
        GUILayout.BeginVertical(GUILayout.Width(width));
        GUILayout.Label(playthrough.TesterName);
        GUILayout.Space(4);
        GUILayout.Label(string.Format("{1} {0}", playthrough.DateTime.ToShortDateString(), playthrough.DateTime.ToShortTimeString()));
        GUILayout.EndVertical();
    }
    private void DrawDurationMovesRetries(float width, Playthrough playthrough)
    {
        GUILayout.BeginVertical(GUILayout.Width(width));
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("{0:D2}:{1:D2}:{2:D2}", playthrough.Duration.Hours, playthrough.Duration.Minutes, playthrough.Duration.Seconds));
        //GUILayout.FlexibleSpace();
        GUILayout.Space(15);
        GUILayout.Label(string.Format("Retries {0}", playthrough.Retries));
        GUILayout.EndHorizontal();
        GUILayout.Space(4);
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("Moves On Win {0}", playthrough.MovesOnWin));
        //GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("Total Moves {0}", playthrough.TotalMoves));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

    }
    private void DrawRatings(float width, Playthrough playthrough)
    {
        float starSize = 20;
        GUILayout.BeginVertical(GUILayout.Width(width));
        if (playthrough.DifficultyRating > 0)
        {
            DrawStarRating("Difficulty", playthrough.DifficultyRating, starSize);
            DrawStarRating("Satisfaction", playthrough.SatisfactionRating, starSize);
        }
        GUILayout.EndVertical();
    }
    private bool DrawButtons(float width, Playthrough playthrough)
    {
        bool result = false;
        GUILayout.BeginVertical(GUILayout.Width(width));
        GUILayout.Space(3);
        if (GUILayout.Button("Delete"))
        {
            result = true;
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("View Realtime"))
        {
            steppedPlayback = false;
            PlaythroughDatabase.Insstance.PlaythroughActionsDownloaded += Insstance_PlaythroughActionsDownloaded;
            PlaythroughDatabase.Insstance.GetPlaythroughActions(playthrough, pWord);
        }
        if (GUILayout.Button("View Stepped"))
        {
            steppedPlayback = true;
            PlaythroughDatabase.Insstance.PlaythroughActionsDownloaded += Insstance_PlaythroughActionsDownloaded;
            PlaythroughDatabase.Insstance.GetPlaythroughActions(playthrough, pWord);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        return result;
    }
    private void DrawStarRating(string name, int rating, float starSize)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(name);
        for (int i = 0; i < 5; i++)
        {
            if (i + 1 <= rating)
                GUILayout.Label(goldStar.texture, GUILayout.Width(starSize), GUILayout.Height(starSize));
            else
                GUILayout.Label(silverStar.texture, GUILayout.Width(starSize), GUILayout.Height(starSize));
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region Delegates
    private void Insstance_PlaythroughsDownloaded(List<Playthrough> playthroughs)
    {
        currentLayout.Playthroughs = playthroughs.OrderBy(x => x.DateTime).ToList();
        PlaythroughDatabase.Insstance.PlaythroughsDownloaded -= Insstance_PlaythroughsDownloaded;
        loading = false;
        Repaint();
    }
    private void Insstance_PlaythroughActionsDownloaded(Playthrough playthrough)
    {
        PlaythroughDatabase.Insstance.PlaythroughActionsDownloaded -= Insstance_PlaythroughActionsDownloaded;
        Gameboard.Instance.GetComponent<BoardLayoutSet>().SetLayout(currentLayout);
        //Gameboard.Instance.ViewReplay(playthrough, steppedPlayback);
    }
    private void Insstance_PlaythroughDeletionComplete()
    {
        PlaythroughDatabase.Insstance.PlaythroughDeletionComplete -= Insstance_PlaythroughDeletionComplete;
        if (currentLayout != null)
            SetLayout(currentLayout);
        else loading = false;
    }
    #endregion
}


