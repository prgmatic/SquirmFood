using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class GameTileLayoutInfo
{
    public byte GameTileID = 0;
    public byte Variation = 0;
    public byte X = 0;
    public byte Y = 0;

    public GameTileLayoutInfo(byte id, byte variation, byte x, byte y)
    {
        this.GameTileID = id;
        this.Variation = variation;
        this.X = x;
        this.Y = y;
    }
}
