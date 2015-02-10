using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeRuleSet : MonoBehaviour
{
    public Sprite TileSelectionSprite;

    [HideInInspector]
    public GameTile SelectedTile = null;

    private Gameboard _gameboard;
    private SpriteRenderer tileSelectRenderer;
    void Start()
    {
        _gameboard = this.GetComponent<Gameboard>();
        GameObject tileSelectGO = new GameObject();
        tileSelectGO.name = "TileSelectionSprite";
        tileSelectGO.transform.SetParent(_gameboard.transform);
        tileSelectRenderer = tileSelectGO.AddComponent<SpriteRenderer>();
        tileSelectRenderer.sprite = TileSelectionSprite;
        tileSelectRenderer.enabled = false;

        _gameboard.TileSettled += _gameboard_TileSettled;
    }

    private void _gameboard_TileSettled(GameTile sender)
    {
        if (sender.Width > 1 || sender.Height > 1) return;
        GameTile[] matchGroup = GetMatches(sender);
        if (matchGroup != null)
        {
            foreach (var match in matchGroup)
            {
                if (match.Moving)
                    return;
            }
            foreach (var match in matchGroup)
            {
                _gameboard.DestroyTile(match);
            }
        }
    }

    public void SelectTile(int x, int y)
    {
        GameTile newSelectedTile = _gameboard.GetTileAt(x, y);
        if(newSelectedTile.Width > 1 || newSelectedTile.Height > 1)
        {
            Deselect();
            return;
        }
        if(SelectedTile != null)
        {
            if(SelectedTile.IsCardinalNeighbor(newSelectedTile))
            {
                StartCoroutine(SwapTiles(SelectedTile, newSelectedTile));
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
    public void SetSelectedTile(GameTile tile)
    {
        SelectedTile = tile;
        tileSelectRenderer.enabled = true;
        tileSelectRenderer.transform.position = tile.WorldPosition;
    }
    public void Deselect()
    {
        SelectedTile = null;
        tileSelectRenderer.enabled = false;
    }

    private void SwapTilesPositions(GameTile tile1, GameTile tile2)
    {
        int tile1X = tile1.GridPosition.x;
        int tile1Y = tile1.GridPosition.y;

        tile1.Move(tile2.GridPosition.x, tile2.GridPosition.y);
        tile2.Move(tile1X, tile1Y);
    }
    System.Collections.IEnumerator SwapTiles(GameTile tile1, GameTile tile2)
    {
        SwapTilesPositions(tile1, tile2);
        while(tile1.Moving || tile2.Moving)
        {
            yield return null;
        }
        var tile1Matches = GetMatches(tile1);
        var tile2Matches = GetMatches(tile2);
        if(tile1Matches == null && tile2Matches == null)
        {
            SwapTilesPositions(tile1, tile2);
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

    public GameTile[] GetMatches(GameTile tile)
    {
        return GetMatches(tile, this._gameboard);
    }
    public static GameTile[] GetMatches(GameTile tile, Gameboard gameboard)
    {

        // check vertical
        List<GameTile> verticalMatches = new List<GameTile>();
        int checkY = tile.GridTop + 1;
        int checkX = tile.GridLeft;
        while (checkY < gameboard.Rows && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check above
        {
            verticalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkY++;
        }
        checkY = tile.GridTop - 1;
        while (checkY >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check below
        {
            verticalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkY--;
        }

        //check horizontal
        List<GameTile> horizontalMatches = new List<GameTile>();
        checkY = tile.GridTop;
        checkX = tile.GridLeft - 1;

        while (checkX >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check left
        {
            horizontalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkX--;
        }
        checkX = tile.GridLeft + 1;
        while (checkX < gameboard.Columns && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check Right
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
    private bool IsMatchAt(string category, int x, int y)
    {
        return IsMatchAt(category, x, y, this._gameboard);
    }
    public static bool IsMatchAt(string category, int x, int y, Gameboard gameboard)
    {
        // check vertical
        int verticalMatches = 0;
        int checkY = y + 1;
        int checkX = x;
        while (checkY < gameboard.Rows && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == category) // Check above
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
        while (checkX < gameboard.Columns && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == category) // Check Right
        {
            horizontalMatches++;
            checkX++;
        }
        if (horizontalMatches > 1)
            return true;

        return false;
    }
}
