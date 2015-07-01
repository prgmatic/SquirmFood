using UnityEngine;
using System.Collections;

public static class WormCreator 
{
    //public static void CreateWorm(Point point)
    //{
    //    CreateWorm(point.x, point.y);
    //}

    //public static void CreateWorm(int x, int y)
    //{
    //    Gameboard.Instance.DestroyTileAt(x, y, false, false);
    //    //var wormTile = Gameboard.Instance.AddTileFromToken(Gameboard.Instance.WormToken, x, y, false, true);
    //    //if (wormTile == null) return null;
    //    //Worm worm = wormTile.gameObject.AddComponent<Worm>();

    //    //var animator = wormTile.gameObject.AddComponent<Animator>();
    //    //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("WormAnimCtrl");
    //    //worm.WormTile = wormTile;
    //    //wormTile.gameObject.AddComponent<WormAnimationSettings>();
    //    //wormTile.transform.localScale = new Vector3(.73f, .73f, 1f);

    //    GameTile wormTile = GameObject.Instantiate(Resources.Load<GameTile>("Worm"));
    //    Gameboard.Instance.AddGameTile(wormTile, x, y);
    //    var worm = wormTile.gameObject.AddComponent<Worm>();
    //    worm.WormTile = wormTile;
    //    return worm;
    //}
}
