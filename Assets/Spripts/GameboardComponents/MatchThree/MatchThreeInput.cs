using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MatchThreeRuleSet))]
public class MatchThreeInput : MonoBehaviour
{
    MatchThreeRuleSet ruleset;
    Point? mouseDownTile = null;
    private bool wasPreviouslySelected = false;

    void Awake()
    {
        ruleset = GetComponent<MatchThreeRuleSet>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            wasPreviouslySelected = false;
            Point gridPosition = Gameboard.Instance.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (Gameboard.Instance.IsValidTileCoordinate(gridPosition))
            {
                GameTile tile = Gameboard.Instance.GetTileAt(gridPosition.x, gridPosition.y);
                if (tile != null)
                {
                    //if(ruleset.SelectedTile == null)
                    if (ruleset.SelectedTile != null && ruleset.SelectedTile.GridPosition == gridPosition)
                        wasPreviouslySelected = true;
                    mouseDownTile = gridPosition;
                    ruleset.SelectTile(gridPosition.x, gridPosition.y);
                }
                else Deselect();
            }
            else Deselect();
        }

        if(mouseDownTile != null)
        {
            Point gridPosition = Gameboard.Instance.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (mouseDownTile.Value != gridPosition)
            {
                if(Gameboard.Instance.IsValidTileCoordinate(gridPosition))
                {
                    if (gridPosition.y < mouseDownTile.Value.y)
                        ruleset.SelectTile(mouseDownTile.Value.x, mouseDownTile.Value.y - 1);
                    else if (gridPosition.y > mouseDownTile.Value.y)
                        ruleset.SelectTile(mouseDownTile.Value.x, mouseDownTile.Value.y + 1);
                    else if (gridPosition.x < mouseDownTile.Value.x)
                        ruleset.SelectTile(mouseDownTile.Value.x - 1, mouseDownTile.Value.y);
                    else if (gridPosition.x > mouseDownTile.Value.x)
                        ruleset.SelectTile(mouseDownTile.Value.x + 1, mouseDownTile.Value.y);

                    mouseDownTile = null;
                }
                else
                    Deselect();
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if (mouseDownTile != null && wasPreviouslySelected)
                Deselect();
            mouseDownTile = null;
        }
    }
/*
        if (Input.GetMouseButtonDown(0))
        {
            Point gridPosition = gameboard.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (gameboard.IsValidTileCoordinate(gridPosition))
            {
                if (gameboard.GetTileAt(gridPosition.x, gridPosition.y) != null)
                {
                    ruleset.SelectTile(gridPosition.x, gridPosition.y);
                }
                else ruleset.Deselect();
            }
            else ruleset.Deselect();
        }
    }
    */

    private void Deselect()
    {
        ruleset.Deselect();
        mouseDownTile = null;
    }
}
