using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class NewBoardLayout : ScriptableObject
{
    const int EXPORT_VERSION = 0;

    public int ID = -1;
    public byte Columns;
    public byte Rows;
    public List<GameTileLayoutInfo> Tiles = new List<GameTileLayoutInfo>();
    public List<int> MudTiles = new List<int>();
    public LevelDifficulty Difficulty = LevelDifficulty.Easy;

    // optional goals
    public bool LimitedMovesGoal = false;
    public int NumberOfMoves = 10;
    public bool EatAllMushroomsGoal = false;

    private static Dictionary<int, GameTile> _tilePrefabs;

    #region StaticMethods

    #region Initialization
    static NewBoardLayout()
    {
        InitTilePrefabs();
    }

    private static void InitTilePrefabs()
    {
        var prefabs = Resources.LoadAll<GameTile>("BoardPieces");
        _tilePrefabs = new Dictionary<int, GameTile>();
        foreach (var prefab in prefabs)
        {
            if (!_tilePrefabs.ContainsKey(prefab.ID))
            {
                _tilePrefabs.Add(prefab.ID, prefab);
            }
        }
    }

    public static GameTile GetPrefab(int id)
    {
        if (_tilePrefabs.ContainsKey(id))
            return _tilePrefabs[id];
        return null;
    }
    #endregion

    #region CreationMethods
    public static NewBoardLayout FromBinary(byte[] data)
    {
        var result = ScriptableObject.CreateInstance<NewBoardLayout>();
        using (MemoryStream ms = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
                // Read header info
                int exportVersion = reader.ReadInt32();
                result.Columns = reader.ReadByte();
                result.Rows = reader.ReadByte(); ;


                // Read tile info
                int numberOfTokens = reader.ReadInt32();
                for (int i = 0; i < numberOfTokens; i++)
                {
                    byte id = reader.ReadByte();
                    byte variation = reader.ReadByte();
                    byte x = reader.ReadByte();
                    byte y = reader.ReadByte();

                    if (id == 0 || id == 255)
                        continue;

                    result.Tiles.Add(new GameTileLayoutInfo(id, variation, x, y));
                }

                // Read Mud info
                int numberOfMudTiles = reader.ReadInt32();
                for (int i = 0; i < numberOfMudTiles; i++)
                {
                    result.MudTiles.Add(reader.ReadInt32());
                }
            }
        }
        return result;
    }

    public static NewBoardLayout FromGameboard()
    {
        var result = ScriptableObject.CreateInstance<NewBoardLayout>();
        foreach (var tile in Gameboard.Instance.gameTiles)
        {
            var worm = tile.GetComponent<Worm>();
            // If the tile is a worm, get it's facing direction and save it under the variation variable
            if (worm != null)
            {
                result.Tiles.Add(new GameTileLayoutInfo((byte)tile.ID, (byte)worm.FacingDirection, (byte)tile.GridLeft, (byte)tile.GridTop));
            }
            else
            {
                result.Tiles.Add(new GameTileLayoutInfo((byte)tile.ID, 0, (byte)tile.GridLeft, (byte)tile.GridTop));
            }
        }
        result.Columns = (byte)Gameboard.Instance.Columns;
        result.Rows = (byte)Gameboard.Instance.Rows;
        for (int y = 0; y < result.Rows; y++)
        {
            for (int x = 0; x < result.Columns; x++)
            {
                if (Gameboard.Instance.GetBackgroundTileAttribute(x, y) == Gameboard.BackgroundTileAttribute.FreeMove)
                {
                    result.MudTiles.Add(x + y * result.Columns);
                }
            }
        }
        return result;
    }
    #endregion

    #endregion

    public void Load()
    {
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
        Gameboard.Instance.CurrentLevel = this;
        if (Columns > 0 && Rows > 0)
        {
            Gameboard.Instance.SetBoardSize(Columns, Rows);
        }

        foreach (var tile in Tiles)
        {
            if (dict.ContainsKey(tile.ID))
            {
                GameTile newTile = Instantiate(dict[tile.ID]);
                Gameboard.Instance.AddGameTile(newTile, tile.X, tile.Y, false);
                var worm = newTile.GetComponent<Worm>();
                // If tile is a worm, load up it's facing direction from the variation variable
                if (worm != null)
                    worm.FacingDirection = (Direction)tile.Variation;
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
        byte[] result = null;
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {

                // Write layout header
                writer.Write(EXPORT_VERSION);
                writer.Write(Columns);
                writer.Write(Rows);

                // Write tile info
                writer.Write(Tiles.Count);
                foreach (var tile in Tiles)
                {
                    if (tile.ID == 0 || tile.ID == 255)
                        Debug.LogError("Tile does not have an ID");
                    writer.Write(tile.ID);
                    writer.Write(tile.Variation);
                    writer.Write(tile.X);
                    writer.Write(tile.Y);
                }

                // Write MudTile info
                writer.Write(MudTiles.Count);
                foreach (var mudTile in MudTiles)
                {
                    writer.Write(mudTile);
                }
                result = ms.ToArray();
            }
        }
        return result;
    }

    public bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }

    public GameTileLayoutInfo GetGameTileInfoAt(int x, int y)
    {
        for (int i = 0; i < Tiles.Count; i++)
        {
            if (Tiles[i].X == x && Tiles[i].Y == y)
            {
                return Tiles[i];
            }
        }
        return null;
    }

    public GameTile GetGameTileAt(int x, int y)
    {
        var info = GetGameTileInfoAt(x, y);
        if (_tilePrefabs.ContainsKey(info.ID))
            return _tilePrefabs[info.ID];
        return null;
    }

    public enum LevelDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}
