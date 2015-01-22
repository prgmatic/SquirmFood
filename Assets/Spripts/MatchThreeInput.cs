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
            int x;
            int y;
            if (gameboard.WorldPositionToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), out x, out y))
            {
                if (gameboard.GetTileAt(x, y) != null)
                {
                    ruleset.SelectTile(x, y);
                }
            }
        }
    }
}
