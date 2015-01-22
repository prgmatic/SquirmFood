using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeRuleSet : MonoBehaviour
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
}
