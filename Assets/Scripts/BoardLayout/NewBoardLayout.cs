using UnityEngine;
using System.Collections.Generic;

public class 
    NewBoardLayout : ScriptableObject
{
    public int ID = -1;
    public byte Columns;
    public byte Rows;
    public List<GameTileLayoutInfo> Tiles = new List<GameTileLayoutInfo>();
    public List<int> MudTiles = new List<int>();

    public static NewBoardLayout FromBinary(byte[] data)
    {
        var result = ScriptableObject.CreateInstance<NewBoardLayout>();

        return result;
    }

    public static NewBoardLayout FromOldBoardLayout(BoardLayout layout)
    {
        var result = ScriptableObject.CreateInstance<NewBoardLayout>();
        result.ID = layout.ID;
        result.Columns = (byte)layout.Columns;
        result.Rows = (byte)layout.Rows;

        foreach (var token in layout.Tokens)
        {
            result.Tiles.Add(new GameTileLayoutInfo(token.Token.ID, (byte)token.Variation, (byte)token.Position.x, (byte)token.Position.y));
        }
        for (int i = 0; i < layout.BackgroundTileAttributes.Length; i++)
        {
            if(layout.BackgroundTileAttributes[i] == Gameboard.BackgroundTileAttribute.FreeMove)
            {
                result.MudTiles.Add(i);
            }
        }
        return result;
    }

    public void Load()
    {
        //var prefabs = Resources.FindObjectsOfTypeAll<GameTile>();
        var prefabs = Resources.LoadAll<GameTile>("BoardPieces");
        var dict = new Dictionary<int, GameTile>();
        foreach (var prefab in prefabs)
        {
            if (!dict.ContainsKey(prefab.ID))
            {
                dict.Add(prefab.ID, prefab);
            }
        }

        Gameboard.Instance.Clear();
        if (Columns > 0 && Rows > 0)
        {
            Gameboard.Instance.SetBoardSize(Columns, Rows);
        }

        foreach (var tile in Tiles)
        {
            if (dict.ContainsKey(tile.GameTileID))
            {
                GameTile newTile = Instantiate(dict[tile.GameTileID]);
                Gameboard.Instance.AddGameTile(newTile, tile.X, tile.Y, false);
            }

        }
        foreach (var mudTile in MudTiles)
        {
            Gameboard.Instance.SetBackgroundTileAttribute(mudTile % Columns, mudTile / Columns, Gameboard.BackgroundTileAttribute.FreeMove);
        }
        Gameboard.Instance.ApplyGravity();
    }

    public byte[] ToBinary()
    {
        return null;
    }
}
