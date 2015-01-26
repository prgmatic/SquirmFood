using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeBoardInitializer : MonoBehaviour
{
    public int NumberOfBodyParts = 3;

    private Gameboard gameboard;
    private List<TileProperties> charms = new List<TileProperties>();
    private List<TileProperties> bodyParts = new List<TileProperties>();

    void Start()
    {
        gameboard = GetComponent<Gameboard>();
        foreach (var tp in gameboard.Tiles)
        {
            if (tp.Width > 1 || tp.Height > 1)
                bodyParts.Add(tp);
            else
                charms.Add(tp);
        }

        AddBodyParts();
        AddCharms();
    }

    private void AddBodyParts()
    {
        if (bodyParts.Count == 0) return;
        int bodyPartsAdded = 0;
        while (bodyPartsAdded < NumberOfBodyParts)
        {
            TileProperties bodyPart = bodyParts[Random.Range(0, bodyParts.Count)];
            bool placed = false;
            while(!placed)
            {
                int x = Random.Range(0, gameboard.Columns - bodyPart.Width + 1);
                int y = Random.Range(0, gameboard.Rows - bodyPart.Height + 1);
                Rectangle bounds = new Rectangle(x, y, bodyPart.Width, bodyPart.Height);
                if(gameboard.NumberOfTilesInBounds(bounds) == 0)
                {
                    gameboard.AddTile(bodyPart, x, y);
                    placed = true;
                    bodyPartsAdded++;
                }
            }
        }
    }
    private void AddCharms()
    {
        for(int y = gameboard.Rows - 1; y >= -gameboard.HopperSize; y--)
        {
            for(int x = 0; x < gameboard.Columns; x++)
            {
                int index = Random.Range(0, charms.Count);
                while (MatchThreeRuleSet.IsMatchAt(charms[index].Category, x, y, gameboard))
                    index = Random.Range(0, charms.Count);
                gameboard.AddTile(charms[index], x, y);
            }
        }
    }
}
