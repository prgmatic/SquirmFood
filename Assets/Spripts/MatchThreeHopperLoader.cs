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
        FillHopper();
        //for(int x = 0; x < sender.Width; x++)
        //{
        //    for(int y = 0; y < sender.Height; y++)
        //    {
        //        gameboard.AddTile(charms[Random.Range(0, charms.Count)], sender.GridLeft + x, -gameboard.HopperSize + sender.Height - 1 - y);
        //    }
        //}
        //gameboard.AddTile(x, -gameboard.HopperSize);
    }

    private bool BodyPartInHopper()
    {
        for (int y = -gameboard.HopperSize; y < 0; y++)
        {
            for (int x = 0; x < gameboard.Columns; x++)
            {
                if (gameboard.GetTileAt(x, y) != null && (gameboard.GetTileAt(x, y).Width > 1 || gameboard.GetTileAt(x, y).Height > 1))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool IsBodyPart(TileProperties tile)
    {
        if (tile.Width > 1 || tile.Height > 1)
            return true;
        return false;
    }

    private void FillHopper()
    {
        Rectangle hopperBounds = new Rectangle(0, -gameboard.HopperSize, gameboard.Columns, gameboard.HopperSize);
        bool bodyPartInHopper = BodyPartInHopper();
        int y = -gameboard.HopperSize;
        for(int x = 0; x < gameboard.Columns; x++)
        {
            if(gameboard.GetTileAt(x, y) == null)
            {
                int index = gameboard.GetTileIndexFromTileSetFromWeightValues();
                while ((bodyPartInHopper && IsBodyPart(gameboard.Tiles[index])) || (x + gameboard.Tiles[index].Width > gameboard.Width || y + gameboard.Tiles[index].Height > 0))
                    index = gameboard.GetTileIndexFromTileSetFromWeightValues();
                if(IsBodyPart(gameboard.Tiles[index]))
                {
                    bodyPartInHopper = true;
                    Rectangle bounds = new Rectangle(x, y, gameboard.Tiles[index].Width, gameboard.Tiles[index].Height);
                    gameboard.DestroyTilesInBounds(bounds, false);
                }
                gameboard.AddTile(index, x, y, true);
            }
        }
    }
}
