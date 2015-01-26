using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Gameboard))]
public class MatchThreeHopperLoader : MonoBehaviour {

    private Gameboard gameboard;
    List<TileProperties> charms = new List<TileProperties>();
    List<TileProperties> bodyParts = new List<TileProperties>();

    void Start()
    {
        gameboard = GetComponent<Gameboard>();
        gameboard.TileDestroyed += Gameboard_TileDestroyed;

        foreach (var tp in gameboard.Tiles)
        {
            if (tp.Width > 1 || tp.Height > 1)
                bodyParts.Add(tp);
            else
                charms.Add(tp);
        }
    }  

    private void Gameboard_TileDestroyed(GameTile sender)
    {
        for(int x = 0; x < sender.Width; x++)
        {
            for(int y = 0; y < sender.Height; y++)
            {
                gameboard.AddTile(charms[Random.Range(0, charms.Count)], sender.GridLeft + x, -gameboard.HopperSize + sender.Height - 1 - y);
            }
        }
        //gameboard.AddTile(x, -gameboard.HopperSize);
    }
}
