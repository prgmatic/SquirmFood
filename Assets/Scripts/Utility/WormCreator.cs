using UnityEngine;
using System.Collections;

public static class WormCreator 
{
    public static Worm CreateWorm(Point point)
    {
        return CreateWorm(point.x, point.y);
    }

	public static Worm CreateWorm(int x, int y)
    {
        Gameboard.Instance.DestroyTileAt(x, y, false, false);
        //var wormTile = Gameboard.Instance.AddTileFromToken(Gameboard.Instance.WormToken, x, y, false, true);
        //if (wormTile == null) return null;
        //Worm worm = wormTile.gameObject.AddComponent<Worm>();

        //var animator = wormTile.gameObject.AddComponent<Animator>();
        //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("WormAnimCtrl");
        //worm.WormTile = wormTile;
        //wormTile.transform.localScale = new Vector3(.73f, .73f, 1f);

        var wormPrefab = Object.Instantiate<GameObject>(Resources.Load<GameObject>("WormPrefab"));
        var tile = wormPrefab.GetComponent<GameTile>();
        var worm = wormPrefab.GetComponent<Worm>();
        tile.GridPosition = new Point(x, y);
        worm.WormTile = tile;

        return worm;
    }
}
