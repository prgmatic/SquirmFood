using UnityEngine;
using System.Collections.Generic;

public class NewBoardLayoutImporter
{
    public static void ImportLayout(NewBoardLayout layout)
    {
        var prefabs = Resources.FindObjectsOfTypeAll<GameTile>();
        var dict = new Dictionary<int, GameTile>();
        foreach (var prefab in prefabs)
        {
            if (!dict.ContainsKey(prefab.ID))
            {
                dict.Add(prefab.ID, prefab);
            }
        }

        Gameboard.Instance.Clear();
        if (layout.Columns > 0 && layout.Rows > 0)
        {
            Gameboard.Instance.SetBoardSize(layout.Columns, layout.Rows);
        }

        foreach (var tile in layout.Tiles)
        {
            if(dict.ContainsKey(tile.GameTileID))
            {
                Gameboard.Instance.AddGameTile(dict[tile.GameTileID], tile.X, tile.Y);
            }

        }
        foreach(var mudTile in layout.MudTiles)
        {
            Gameboard.Instance.SetBackgroundTileAttribute(mudTile % layout.Rows, mudTile / layout.Rows, Gameboard.BackgroundTileAttribute.FreeMove);
        }
        Gameboard.Instance.ApplyGravity();
    }
}
