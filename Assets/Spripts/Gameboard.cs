using UnityEngine;
using System.Collections.Generic;

public class Gameboard : MonoBehaviour
{
    public int TilesPerRow = 8;
    public int TilesPerComlumn = 9;
    public TileSet TileSet;
    public Color BackGroundColor1 = new Color(32f / 255, 30f / 255, 24f / 255);
    public Color BackGroundColor2 = new Color(63f / 255, 51f / 255, 47f / 255);


    public float Left { get { return transform.position.x - (float)TilesPerRow / 2f * TileSet.TileWidth; } }
    public float Right { get { return transform.position.x + (float)TilesPerRow / 2f * TileSet.TileWidth; } }
    public float Top { get { return transform.position.y + (float)TilesPerComlumn / 2f * TileSet.TileHeight; } }
    public float Bottom { get { return transform.position.y - (float)TilesPerComlumn / 2f * TileSet.TileHeight; } }

    private GameTile[,] tileTable;
    private List<GameTile> gameTiles = new List<GameTile>();
    private RuleSet[] ruleSets;
    

    void Awake()
    {
        tileTable = new GameTile[TilesPerRow, TilesPerComlumn];
        TileSet.GameboardReference = this;
        CreateBackgroundTiles();
        ruleSets = GetComponents<RuleSet>();
    }

    public void AddTile(GameTile tile, int x, int y)
    {

        for (int w = 0; w < tile.Width; w++) // If a tile is already in this space, destory the tile
        {
            for (int h = 0; h < tile.Height; h++)
            {
                if (!IsValidTileCoordinate(x + w, y + h) || GetTileAt(x + w, y + h) != null)
                {
                    Destroy(tile.gameObject);
                    return;
                }
            }
        }        
        
        tile.transform.SetParent(this.transform);
        tile.SetPosition(x, y);
        gameTiles.Add(tile);
        tile.SettledFromFall += Tile_Settled;

        Point[] gridSpaces = tile.GridSpacesOccupied();
        foreach(var point in gridSpaces)
        {
            tileTable[point.x, point.y] = tile;
        }
    }

    private void Tile_Settled(GameTile sender)
    {
        foreach(var ruleset in ruleSets)
        {
            ruleset.OnTileSettled(sender);
        }
    }

    public bool WorldPositionToGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = (int)((worldPosition.x - Left) / TileSet.TileWidth);
        y = -(int)((worldPosition.y - Top) / TileSet.TileHeight);
        return IsValidTileCoordinate(x, y);
    }

    public Point WorldPositionToGridPosition(Vector3 worldPosition)
    {
        int x = (int)((worldPosition.x - Left) / TileSet.TileWidth);
        int y = -(int)((worldPosition.y - Top) / TileSet.TileHeight);
        return new Point(x, y);
    }

    public GameTile GetTileAt(int x, int y)
    {
        try
        {
            return tileTable[x, y];
        }
        catch
        {
            
            return null;
        }
    }

    public void MoveTile(GameTile tile, Point[] oldSpaces, Point[] newSpace)
    {
        foreach(var point in oldSpaces)
        {
            tileTable[point.x, point.y] = null;
        }
        foreach (var point in newSpace)
        {
            tileTable[point.x, point.y] = tile;
        }
        tile.X = newSpace[0].x;
        tile.Y = newSpace[0].y;
    }
    public void MoveTile(GameTile tile, int x, int y)
    {
        MoveTile(tile.X, tile.Y, x, y);
    }
    public void MoveTile(int oldX, int oldY, int newX, int newY)
    {
        if (newX != oldX || newY != oldY)
        {

            tileTable[newX, newY] = tileTable[oldX, oldY];
            tileTable[oldX, oldY] = null;
            tileTable[newX, newY].X = newX;
            tileTable[newX, newY].Y = newY;
        }
    }
    /*
    private void ApplyGravityToColumn(int x)
    {
        int yPos = TilesPerComlumn - 1;

        while (yPos >= 0)
        {
            if (tileTable[x, yPos] != null)
            {
                GameTile tile = tileTable[x, yPos];
                Point[] occupiedGridSpace = tile.GridSpacesOccupied();
                foreach(var point in occupiedGridSpace)
                {
                    tileTable[point.x, point.y] = null;
                }
                Point[] newGridSpace = tile.GridSpacesAfterApplyingGravity();
                foreach(var point in newGridSpace)
                {
                    tileTable[point.x, point.y] = tile;
                }
                tile.X = newGridSpace[0].x;
                tile.Y = newGridSpace[0].y;
                tile.Fall();
                //MoveTile(x, yPos, x, nextFreeSpace);

                

                //tileTable[x, nextFreeSpace].Fall();
                //nextFreeSpace--;
            }
            yPos--;
        }
    }
    */
    public void SwapTiles(GameTile tile1, GameTile tile2)
    {
        int tile1X = tile1.X;
        int tile1Y = tile1.Y;

        tile1.X = tile2.X;
        tile1.Y = tile2.Y;
        tileTable[tile2.X, tile2.Y] = tile1;

        tile2.X = tile1X;
        tile2.Y = tile1Y;
        tileTable[tile1X, tile1Y] = tile2;

        tile1.UpdatePosition();
        tile2.UpdatePosition();
    }
    public void DestroyTile(GameTile tile)
    {
        DestroyTileAt(tile.X, tile.Y);
    }
    public void DestroyTileAt(int x, int y)
    {
        if (!IsValidTileCoordinate(x, y)) return;
        if (tileTable[x, y] == null) return;

        GameTile tile = tileTable[x, y];
        gameTiles.Remove(tile);
        tileTable[x, y] = null;
        Destroy(tile.gameObject);

        //ApplyGravityToColumn(x);
    }

    public bool IsValidTileCoordinate(int x, int y)
    {
        return x >= 0 && x < TilesPerRow && y >= 0 && y < TilesPerComlumn;
    }

    private void CreateBackgroundTiles()
    {
        GameObject backgroundTiles = new GameObject();
        backgroundTiles.name = "BackgroundTiles";
        backgroundTiles.transform.SetParent(this.transform);

        for(int y = 0; y < TilesPerComlumn; y++)
        {
            for(int x = 0; x < TilesPerRow; x++)
            {
                GameObject backgroundTile = new GameObject();
                backgroundTile.name = "BackgroundTile";
                backgroundTile.transform.SetParent(backgroundTiles.transform);
                SpriteRenderer bgRenderer = backgroundTile.AddComponent<SpriteRenderer>();
                bgRenderer.sprite = Utils.DummySprite;
                bgRenderer.transform.localScale = new Vector3(TileSet.TileWidth * 100, TileSet.TileHeight * 100, 1);
                bgRenderer.transform.position = new Vector3(
                    Left + x * TileSet.TileWidth + TileSet.TileWidth / 2,
                    Top - y * TileSet.TileHeight - TileSet.TileHeight / 2
                    );
                bgRenderer.color = (x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1) ? BackGroundColor1 : BackGroundColor2;
            }
        }
    }
    public Vector3 GridPositionToWorldPosition(int x, int y)
    {
        return new Vector3(
            Left + TileSet.TileWidth * x + TileSet.TileWidth / 2,
            Top - TileSet.TileHeight * y - TileSet.TileHeight / 2, 0
            );
    }

}
