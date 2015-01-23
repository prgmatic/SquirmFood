using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeRuleSet : RuleSet
{
    public Sprite TileSelectionSprite;

    [HideInInspector]
    public GameTile SelectedTile = null;

    private Gameboard gameboard;
    private SpriteRenderer tileSelectRenderer;

    void Awake()
    {
        gameboard = this.GetComponent<Gameboard>();
        GameObject tileSelectGO = new GameObject();
        tileSelectGO.name = "TileSelectionSprite";
        tileSelectGO.transform.SetParent(gameboard.transform);
        tileSelectRenderer = tileSelectGO.AddComponent<SpriteRenderer>();
        tileSelectRenderer.sprite = TileSelectionSprite;
        tileSelectRenderer.enabled = false;
    }

    public void SelectTile(int x, int y)
    {
        GameTile newSelectedTile = gameboard.GetTileAt(x, y);
        if(SelectedTile != null)
        {
            if(SelectedTile.IsCardinalNeighbor(newSelectedTile))
            {
                gameboard.SwapTiles(SelectedTile, newSelectedTile);
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
            gameboard.Left + tile.X * gameboard.TileSet.TileWidth + gameboard.TileSet.TileWidth / 2,
            gameboard.Top - tile.Y * gameboard.TileSet.TileHeight - gameboard.TileSet.TileHeight / 2);
    }

    public override void OnTileSettled(GameTile tile)
    {
         CheckForMatch(tile);
        
    }

    public void CheckForMatch(GameTile tile)
    {
        // check vertical
        List<GameTile> verticalMatches = new List<GameTile>();
        int checkY = tile.Y + 1;
        int checkX = tile.X;
        while (checkY < gameboard.TilesPerComlumn && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check above
        {
            if (gameboard.GetTileAt(checkX, checkY).Moving) return;
            verticalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkY++;
        }
        checkY = tile.Y - 1;
        while (checkY >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check below
        {
            if (gameboard.GetTileAt(checkX, checkY).Moving) return;
            verticalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkY--;
        }

        //check horizontal
        List<GameTile> horizontalMatches = new List<GameTile>();
        checkY = tile.Y;
        checkX = tile.X - 1;

        while (checkX >= 0 && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check left
        {
            if (gameboard.GetTileAt(checkX, checkY).Moving) return;
            horizontalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkX--;
        }
        checkX = tile.X + 1;
        while (checkX < gameboard.TilesPerRow && gameboard.GetTileAt(checkX, checkY) != null && gameboard.GetTileAt(checkX, checkY).Category == tile.Category) // Check Right
        {
            if (gameboard.GetTileAt(checkX, checkY).Moving) return;
            horizontalMatches.Add(gameboard.GetTileAt(checkX, checkY));
            checkX++;
        }
        if(horizontalMatches.Count > 1 || verticalMatches.Count > 1)
        {
            gameboard.DestroyTile(tile);
        }
        if(horizontalMatches.Count > 1)
        {
            foreach (var t in horizontalMatches)
                gameboard.DestroyTile(t);
        }
        if(verticalMatches.Count > 1)
        {
            foreach (var t in verticalMatches)
                gameboard.DestroyTile(t);
        }
    }
}
