using UnityEngine;
using System.Collections;

public static class WormCreator 
{
    public static Worm CreateWorm(Token headToke, Token bodyToken, Point point, WormProperties properties)
    {
        return CreateWorm(headToke, bodyToken, point.x, point.y, properties);
    }

	public static Worm CreateWorm(Token headToken, Token bodyToken, int x, int y, WormProperties properties)
    {
        var head = Gameboard.Instance.AddTileFromToken(headToken, x, y, false, true);
        if (head == null) return null;
        Worm worm = head.gameObject.AddComponent<Worm>();
        worm.SetProperties(properties);
        worm.SectionToken = bodyToken;
        return worm;
    }
}
