using UnityEngine;
using System.Collections.Generic;

public class Gameboard : MonoBehaviour
{
    public int TilesPerRow = 8;
    public int TilesPerComlumn = 9;
    public TileSet TileSet;
    public Color BackGroundColor1 = new Color(32f / 255, 30f / 255, 24f / 255);
    public Color BackGroundColor2 = new Color(63f / 255, 51f / 255, 47f / 255);
    

    private GameTile selectedTile = null;

    public float Left { get { return transform.position.x - (float)TilesPerRow / 2f * TileSet.TileWidth; } }
    public float Right { get { return transform.position.x + (float)TilesPerRow / 2f * TileSet.TileWidth; } }
    public float Top { get { return transform.position.y + (float)TilesPerComlumn / 2f * TileSet.TileHeight; } }
    public float Bottom { get { return transform.position.y - (float)TilesPerComlumn / 2f * TileSet.TileHeight; } }


    private List<GameTile> gameTiles = new List<GameTile>();

    

    void Awake()
    {
        TileSet.GameboardReference = this;

        CreateBackgroundTiles();
    }

    public void AddTile(GameTile tile, int x, int y)
    {
        tile.transform.SetParent(this.transform);
        tile.transform.localPosition = new Vector3(this.Left + TileSet.TileWidth * x + TileSet.TileWidth / 2, this.Top - TileSet.TileHeight * y - TileSet.TileHeight / 2);
        gameTiles.Add(tile);
    }

    public bool WorldPositionToGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = (int)((worldPosition.x - Left) / TileSet.TileWidth);
        y = -(int)((worldPosition.y - Top) / TileSet.TileHeight);
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

    void SelectTile(GameTile tile)
    {
        int index = gameTiles.IndexOf(tile);
        GameTile tileToSelect = gameTiles[index];

        if(selectedTile != null)
        {
            int selectedTileIndex = gameTiles.IndexOf(selectedTile);
            gameTiles[selectedTileIndex] = tileToSelect;
            gameTiles[index] = selectedTile;

            Vector3 tileToSelectPosition = tileToSelect.transform.position;
            //tileToSelect.transform.position = selectedTile.transform.position;
            //selectedTile.transform.position = tileToSelectPosition;

            tileToSelect.MoveTo(selectedTile.transform.position);
            selectedTile.MoveTo(tileToSelectPosition);

            selectedTile = null;
        }
        else
            selectedTile = tileToSelect;
    }

    /*
    void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero);
            if(hit.collider != null)
            {
                SelectTile(hit.collider.gameObject.GetComponent<GameTile>());
                //Debug.Log(hit.transform.name);
            }
        }
    }
    */

    private GameTile TileAt(int x, int y)
    {
        return gameTiles[x + y * TilesPerRow];
    }

    /*
    private bool CheckForMatch(int x, int y)
    {
        // check vertical
        GameTile currentTile = TileAt(x, y);
        int verticalMatches = 0;
        int checkY = y + 1;
        int checkX = x;
        while (checkY < TilesPerComlumn && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check above
        {
            verticalMatches++;
            checkY++;
        }
        checkY = y - 1;
        while (checkY >= 0 && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check below
        {
            verticalMatches++;
            checkY--;
        }
        if (verticalMatches > 1)
            return true;

        int horizontalMatches = 0;
        checkY = y;
        checkX = x - 1;

        while (checkX >= 0 && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check left
        {
            horizontalMatches++;
            checkX--;
        }
        checkX = x + 1;
        while (checkX < TilesPerRow && TileAt(checkX, checkY) != null && TileAt(checkX, checkY).TileCategory == currentTile.TileCategory) // Check Right
        {
            horizontalMatches++;
            checkX++;
        }
        if (horizontalMatches > 1)
            return true;

        return false;
    }
    */

}
