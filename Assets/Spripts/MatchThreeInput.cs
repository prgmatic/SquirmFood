using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MatchThreeRuleSet))]
public class MatchThreeInput : MonoBehaviour
{
    MatchThreeRuleSet ruleset;

    void Awake()
    {
        ruleset = GetComponent<MatchThreeRuleSet>();
    }

	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero);
            
            if (hit.collider != null)
            {
                GameTile tile = hit.collider.GetComponent<GameTile>();
                if(tile != null)
                {
                    Debug.Log(tile.X + " " + tile.Y);
                    ruleset.SelectTile(tile.X, tile.Y);
                }
            }
        }
    }
}
