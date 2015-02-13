using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GameTile))]
public class Worm : MonoBehaviour 
{
    public int StomachSize = 7;
    public int Length = 4;
    public int MovesBeforeDeath;
    public bool CanMoveOverSelf;
    public bool DieWhenStomachFull;
    public bool OnlyMoveOnEat;
    public bool DisplayMovesTaken;
    [HideInInspector]
    public List<GameTile> Sections = new List<GameTile>();
    [HideInInspector]
    public Token SectionToken;
    [HideInInspector]
    public List<Token> Stomach = new List<Token>();

    private int _movesTaken = 0;

    public Point MovingTo = Point.zero;

    public GameTile Head { get { return Sections[0]; } }
    public GameTile Tail { get { return Sections[Sections.Count - 1]; } }
    public bool IsStomachFull { get { return Stomach.Count >= StomachSize; } }
    public int MovesTaken { get { return _movesTaken; } }

    void Awake()
    {
        Sections.Add(GetComponent<GameTile>());
        DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
    }

    private void DebugHUD_MessagesCleared(object sender, System.EventArgs e)
    {
        if (DisplayMovesTaken)
        {
            DebugHUD.Add("Moves Taken: " + _movesTaken);
        }
    }

    public void SetProperties(WormProperties properties)
    {
        this.Length = properties.Length;
        this.StomachSize = properties.StomachSize;
        this.CanMoveOverSelf = properties.CanMoveOverSelf;
        this.DieWhenStomachFull = properties.DieWhenStomachFull;
        this.OnlyMoveOnEat = properties.OnlyMoveOnEat;
        this.DisplayMovesTaken = properties.DisplayMovesTaken;
        this.MovesBeforeDeath = properties.MovesBeforeDeath;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Kill();
        }
        while(Sections.Count > Length)
            RemoveTail();
    }

    

    public void Move(int x, int y)
    {
        if (Gameboard.Instance.IsValidTileCoordinate(x, y, true))
        {
            GameTile tile = Gameboard.Instance.GetTileAt(x, y);
            if (tile != null)
            {
                if (tile.IsEdible && !IsStomachFull)
                {
                    //tileToDestory = tile;
                    MovingTo = new Point(x, y);
                    EatToken(tile);
                }
                else if(tile.Pushable)
                {
                    Direction direction = Direction.Right;
                    if (x < Head.GridPosition.x) direction = Direction.Left;
                    else if (y > Head.GridPosition.y) direction = Direction.Down;
                    else if (y < Head.GridPosition.y) direction = Direction.Up;
                    if (!tile.Push(direction)) return;
                }
                else if (!tile.IsWorm || !CanMoveOverSelf) return;
            }
            else if (OnlyMoveOnEat) return;
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
        _movesTaken++;
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
        

        if(MovesBeforeDeath > 0 && _movesTaken >= MovesBeforeDeath)
        {
            Kill();
        }

        Gameboard.Instance.ApplyGravity();

    }

    private void RemoveTail()
    {
        GameTile tail = Tail;
        Sections.Remove(Tail);
        Gameboard.Instance.DestroyTile(tail);
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
        DebugHUD.MessagesCleared -= DebugHUD_MessagesCleared;
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
    public int MovesBeforeDeath = 0;
    public bool CanMoveOverSelf = true;
    public bool DieWhenStomachFull = true;
    public bool OnlyMoveOnEat = false;
    public bool DisplayMovesTaken = true;
}
