using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateToken : MonoBehaviour
{
    const string DefaultGameActionsFolder = @"Assets\Actions";
    const string DefaultRecipesFolder = @"Assets\Recipes";

    [MenuItem("Monster Mashup/Create Token/Colored")]
    static void CreateColoredToken()
    {
        ColoredToken coloredToken = ScriptableObject.CreateInstance<ColoredToken>();
        AssetDatabase.CreateAsset(coloredToken, "Assets/ColoredToken.asset");
        AssetDatabase.Refresh();
        Debug.Log("Creating new colored token.");
    }

    [MenuItem("Monster Mashup/Create Token/Textured")]
    static void CreateTexturedToken()
    {
        TexturedToken textureToken = ScriptableObject.CreateInstance<TexturedToken>();
        AssetDatabase.CreateAsset(textureToken, "Assets/TexturedToken.asset");
        AssetDatabase.Refresh();
        Debug.Log("Creating new textured token.");
    }

    [MenuItem("Monster Mashup/Create Action/Modify Worm Size")]
    static void CreateModifyWormSizeAction()
    {
        ModifyWormSize action = ScriptableObject.CreateInstance<ModifyWormSize>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\ModifyWormSize.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Monster Mashup/Create Action/Modify Worm Size And Drop Token")]
    static void CreateModifyWormSizeAndDropTokenAction()
    {
        ModifyWormSizeAndDropToken action = ScriptableObject.CreateInstance<ModifyWormSizeAndDropToken>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\ModifyWormSizeAndDropToken.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Monster Mashup/Create Action/Modify Worm Stomach Size")]
    static void CreateModifyWormStomachSizeAction()
    {
        ModifyWormStomachSize action = ScriptableObject.CreateInstance<ModifyWormStomachSize>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\ModifyWormStomachSize.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("Monster Mashup/Create Action/Drop Token")]
    static void CreateDropTokenAction()
    {
        DropToken action = ScriptableObject.CreateInstance<DropToken>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\DropToken.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Monster Mashup/Create Recipe")]
    static void CreateRecipe()
    {
        Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
        CreateDirectory(DefaultRecipesFolder);
        AssetDatabase.CreateAsset(recipe, DefaultRecipesFolder + @"\NewRecipe.asset");
        AssetDatabase.Refresh();
    }
    

    private static void CreateDirectory(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }
}
