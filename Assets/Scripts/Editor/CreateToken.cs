using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateToken : MonoBehaviour
{
    const string DefaultGameActionsFolder = @"Assets\Actions";
    const string DefaultRecipesFolder = @"Assets\Recipes";
    const string DefaultConditionsFolder = @"Assets\Conditions";

    #region CreateToken
    [MenuItem("Worm Food/Create Token/Colored")]
    static void CreateColoredToken()
    {
        ColoredToken coloredToken = ScriptableObject.CreateInstance<ColoredToken>();
        AssetDatabase.CreateAsset(coloredToken, "Assets/ColoredToken.asset");
        AssetDatabase.Refresh();
        Debug.Log("Creating new colored token.");
    }

    [MenuItem("Worm Food/Create Token/Textured")]
    static void CreateTexturedToken()
    {
        TexturedToken textureToken = ScriptableObject.CreateInstance<TexturedToken>();
        AssetDatabase.CreateAsset(textureToken, "Assets/TexturedToken.asset");
        AssetDatabase.Refresh();
        Debug.Log("Creating new textured token.");
    }
    #endregion

    private static void CreateDirectory(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }
}
