using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeRuleSet : RuleSet
{
    public Sprite TileSelectionSprite;

    [HideInInspector]
    public GameTile SelectedTile = null;

    private Gameboard _gameboard;
    private SpriteRenderer tileSelectRenderer;

    void Awake()
    {
        _gameboard = this.GetComponent<Gameboard>();
        GameObject tileSelectGO = new GameObject();
        tileSelectGO.name = "TileSelectionSprite";
        tileSelectGO.transform.SetParent(_gameboard.transform);
        tileSelectRenderer = tileSelectGO.AddComponent<SpriteRenderer>();
        tileSelectRenderer.sprite = TileSelectionSprite;
        tileSelectRenderer.enabled = false;
    }

    public void SelectTile(int x, int y)
    {
        GameTile newSelectedTile = _gameboard.GetTileAt(x, y);
        if(SelectedTile != null)
        {
            if(SelectedTile.IsCardinalNeighbor(newSelectedTile))
            {
                StartCoroutine(SwapTiles(SelectedTile, newSelectedTile));
                SwapTiles(SelectedTile, newSelectedTile);
                Deselect();
            }
            else
            {
                SetSelectedTile(newSelectedTile);
            }
        }
        else
        {
            SetSelectedTile(newSelectedTile);
        }
    }

    System.Collections.IEnumerator SwapTiles(GameTile tile1, GameTile tile2)
    {
        _gameboard.SwapTiles(tile1, tile2);
        while(tile1.Moving || tile2.Moving)
        {
            yield return null;
        }
        var tile1Matches = CheckForMatch(tile1);
        var tile2Matches = CheckForMatch(tile2);
        if(tile1Matches == null && CheckForMatch(tile2) == null)
        {
            Debug.Log("No match");
            _gameboard.SwapTiles(tile1, tile2);
        }
        if(tile1Matches != null)
        {
            foreach (var tile in tile1Matches)
                _gameboard.DestroyTile(tile);
        }
        if (tile2Matches != null)
        {
            foreach (var tile in tile2Matches)
                _gameboard.DestroyTile(tile);
        }

    }

    public bool IsSwapValid(GameTile tile1, GameTile tile2)
    {
        if (!tile1.IsCardinalNeighbor(tile2)) return false;
        return CheckForMatch(tile1.Category, tile2.X, tile2.Y) || CheckForMatch(tile2.Category, tile1.X, tile1.Y);
    }
    public void Deselect()
    {
        SelectedTile = null;
        tileSelectRenderer.enabled = false;
    }
    public void SetSelectedTile(GameTile tile)
    {
        SelectedTile = tile;
        tileSelectRenderer.enabled = true;
        tileSelectRenderer.transform.position = new Vector3(
            _gameboard.Left + tile.X * _gameboard.TileSet.TileWidth + _gameboard.TileSet.TileWidth / 2,
            _gameboard.Top - tile.Y * _gameboard.TileSet.TileHeight - _gameboard.TileSet.TileHeight / 2);
    }

    public override void OnTileSettled(GameTile tile)
    {
        GameTile[] matchGroup = CheckForMatch(tile);
        if(matchGroup != null)
        {
            foreach(var match in matchGroup)
            {
                if (match.Moving)
                    return;
            }
            foreach(var match in matchGroup)
            {
                _gameboard.DestroyTile(match);
            }
        }
    }
    public GameTile[] CheckForMatch(GameTile tile)
    {
        return CheckForMatch(tile, this._gameboard);
    }

    public static GameTile[] CheckForMatch(GameTile tile, Gameboard gameboard)
    {

        // check vertical
        List<GameTile> verticalMatches = new List<GameTile>();
        int checkY = tile.Y + 1;
        int checkX = tile.X;
        while (checkY < gameboard.TilesPerComlumn && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check above
        {
            verticalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkY++;
        }
        checkY = tile.Y - 1;
        while (checkY >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check below
        {
            verticalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkY--;
        }

        //check horizontal
        List<GameTile> horizontalMatches = new List<GameTile>();
        checkY = tile.Y;
        checkX = tile.X - 1;

        while (checkX >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check left
        {
            horizontalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkX--;
        }
        checkX = tile.X + 1;
        while (checkX < gameboard.TilesPerRow && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check Right
        {
            horizontalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkX++;
        }

        List<GameTile> matchGroup = new List<GameTile>();
        if(horizontalMatches.Count > 1 || verticalMatches.Count > 1)
        {
            matchGroup.Add(tile);
        }
        if(horizontalMatches.Count > 1)
        {
            foreach (var t in horizontalMatches)
                matchGroup.Add(t);
        }
        if(verticalMatches.Count > 1)
        {
            foreach (var t in verticalMatches)
                matchGroup.Add(t);
        }
        if (matchGroup.Count > 0)
            return matchGroup.ToArray();
        else
            return null;
    }
    private bool CheckForMatch(string category, int x, int y)
    {
        return CheckForMatch(category, x, y, this._gameboard);
    }
    public static bool CheckForMatch(string category, int x, int y, Gameboard gameboard)
    {
        // check vertical
        int verticalMatches = 0;
        int checkY = y + 1;
        int checkX = x;
        while (checkY < gameboard.TilesPerComlumn && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == category) // Check above
        {
            verticalMatches++;
            checkY++;
        }
        checkY = y - 1;
        while (checkY >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == category) // Check below
        {
            verticalMatches++;
            checkY--;
        }
        if (verticalMatches > 1)
            return true;

        int horizontalMatches = 0;
        checkY = y;
        checkX = x - 1;

        while (checkX >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == category) // Check left
        {
            horizontalMatches++;
            checkX--;
        }
        checkX = x + 1;
        while (checkX < gameboard.TilesPerRow && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == category) // Check Right
        {
            horizontalMatches++;
            checkX++;
        }
        if (horizontalMatches > 1)
            return true;

        return false;
    }
}
