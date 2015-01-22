using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateTileSet : MonoBehaviour
{
    [MenuItem ("Monster Mashup/Create Tile Set")]
    static void CreateIt()
    {
        TileSet tileSet = ScriptableObject.CreateInstance<TileSet>();
        AssetDatabase.CreateAsset(tileSet, "Assets/NewTileSet.asset");
        AssetDatabase.Refresh();
        Debug.Log("Creating new tile set.");
    }

}
