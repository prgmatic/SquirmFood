using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameTile))]
public class Worm : MonoBehaviour 
{
    public int StomachSize = 7;
    public int Length = 4;
    public bool CanMoveOverSelf;
    public bool DieWhenStomachFull;
    [HideInInspector]
    public List<GameTile> Sections = new List<GameTile>();
    [HideInInspector]
    public Token SectionToken;
    [HideInInspector]
    public List<Token> Stomach = new List<Token>();

    

    public GameTile Head { get { return Sections[0]; } }
    public GameTile Tail { get { return Sections[Sections.Count - 1]; } }
    public bool IsStomachFull { get { return Stomach.Count >= StomachSize; } }

    void Awake()
    {
        Sections.Add(GetComponent<GameTile>());
    }

    public void SetProperties(WormProperties properties)
    {
        this.Length = properties.Length;
        this.StomachSize = properties.StomachSize;
        this.CanMoveOverSelf = properties.CanMoveOverSelf;
        this.DieWhenStomachFull = properties.DieWhenStomachFull;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Kill();
        }
    }

    public void Move(int x, int y)
    {
        if (Gameboard.Instance.IsValidTileCoordinate(x, y, true))
        {
            GameTile tile = Gameboard.Instance.GetTileAt(x, y);
            if(tile != null)
            {
                if(tile.IsEdible && !IsStomachFull)
                {
                    //tileToDestory = tile;
                    EatToken(tile);
                }
                else if (!tile.IsWorm || !CanMoveOverSelf) return;
            }
        }
        if (Sections.Count == 0) return; // Worm has died
        try
        {
            Point testPoint = Tail.GridPosition;
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex);
        }

        Point previousTailPosition = Tail.GridPosition;

        Point prevPos = Head.GridPosition;
        Head.Move(x, y, true);
        for(int i = 1;i < Sections.Count; i++)
        {
            Point sectionPrevPos = Sections[i].GridPosition;
            Sections[i].Move(prevPos.x, prevPos.y);
            prevPos = sectionPrevPos;
        }
        if (Sections.Count < Length)
        {
            Sections.Add(Gameboard.Instance.AddTileFromToken(SectionToken, previousTailPosition, false, true));
        }
        Gameboard.Instance.ApplyGravity();

    }

    public void EatToken(GameTile tile)
    {
        Stomach.Add(tile.TokenProperties);
        int matchingTokens = ActiveRecipes.Instance.CheckForMatches(this);
        RemoveFromEndOfStomach(matchingTokens);
        Gameboard.Instance.DestroyTile(tile, true, false);
        if(IsStomachFull && DieWhenStomachFull)
        {
            Kill();
        }
        else
            WormStomach.Instance.SetWorm(this);
    }

    private void RemoveFromEndOfStomach(int amount)
    {
        Stomach.RemoveRange(Stomach.Count - amount, amount);
    }

    public void Kill()
    {
        WormStomach.Instance.SetWorm(null);
        while(Sections.Count > 0)
        {
            Gameboard.Instance.DestroyTile(Sections[Sections.Count - 1]);
            Sections.RemoveAt(Sections.Count - 1);
        }
        /*
        List<GameTile> sections = this.Sections;
        foreach(var section in sections)
        {
            Gameboard.Instance.DestroyTile(section, false, false);
        }
        */
        Gameboard.Instance.ApplyGravity();
    }
}

[System.Serializable]
public class WormProperties
{
    public int Length = 4;
    public int StomachSize = 7;
    public bool CanMoveOverSelf = true;
    public bool DieWhenStomachFull = true;
}
