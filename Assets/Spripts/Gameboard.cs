using UnityEngine;
using System.Collections.Generic;

public class Gameboard : MonoBehaviour
{
    public int TilesPerRow = 8;
    public int TilesPerComlumn = 9;


    public float TileWidth = .54f;
    public float TileHeight = .54f;

    public List<TileCategorySet> tileCategories;
          
    

    private GameTile selectedTile = null;

    public float Left
    {
        get { return transform.position.x - (float)TilesPerRow / 2f * TileWidth; }
    }
    public float Right
    {
        get { return transform.position.x + (float)TilesPerRow / 2f * TileWidth; }
    }

    public float Top
    {
        get { return transform.position.y + TilesPerComlumn * TileHeight; }
    }

    public float Bottom
    {
        get { return transform.position.y; }
    }


    private List<GameTile> gameTiles;

    void Awake()
    {
        gameTiles = new List<GameTile>();
        for(int i = 0; i < TilesPerComlumn * TilesPerRow; i++)
        {
            gameTiles.Add(null);
        }

        for(int y = 0; y < TilesPerComlumn; y++)
        {
            for (int x = 0; x < TilesPerRow; x++)
            {
                Vector3 position = new Vector3(Left + x * TileWidth, Bottom + y * TileHeight);
                //GameTile tile = (GameTile)Instantiate(TilePrefab, position, Quaternion.identity);
                GameObject go = new GameObject("Tile");
                BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
                bc.size = new Vector2(.54f, .54f);
                GameTile tile = go.AddComponent<GameTile>();

                tile.transform.SetParent(this.transform, false);
                tile.SetGameboard(this);
                tile.transform.position = position;
                gameTiles[x + y * TilesPerRow] = tile;

                int categoryIndex = Random.Range(0, tileCategories.Count);
                tile.TileCategory = tileCategories[categoryIndex].TileCategory;
                tile.Color = tileCategories[categoryIndex].Color;

                while (CheckForMatch(x, y))
                {
                    categoryIndex = Random.Range(0, tileCategories.Count);
                    tile.TileCategory = tileCategories[categoryIndex].TileCategory;
                    tile.Color = tileCategories[categoryIndex].Color;
                }
                tile.SetSprite(tileCategories[categoryIndex].Spites[Random.Range(0, tileCategories[categoryIndex].Spites.Count)]);

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

    private GameTile TileAt(int x, int y)
    {
        return gameTiles[x + y * TilesPerRow];
    }

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

}
