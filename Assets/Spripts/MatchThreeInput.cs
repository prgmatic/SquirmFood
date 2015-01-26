using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MatchThreeRuleSet))]
public class MatchThreeInput : MonoBehaviour
{
    Gameboard gameboard;
    MatchThreeRuleSet ruleset;

    void Awake()
    {
        gameboard = GetComponent<Gameboard>();
        ruleset = GetComponent<MatchThreeRuleSet>();
    }
	void Update()
    {
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
}
