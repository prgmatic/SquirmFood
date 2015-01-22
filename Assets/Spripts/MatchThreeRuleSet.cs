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
        tileSelectRenderer.enabled = true;
        tileSelectRenderer.transform.position = new Vector3(
            gameboard.Left + x * gameboard.TileSet.TileWidth + gameboard.TileSet.TileWidth / 2, 
            gameboard.Top - y * gameboard.TileSet.TileHeight - gameboard.TileSet.TileHeight / 2);
    }

    public override void OnTileSettled(GameTile tile)
    {
        CheckForMatch(tile);
        
    }

    public void CheckForMatch(GameTile tile)
    {
        Debug.Log("Check for match");

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
