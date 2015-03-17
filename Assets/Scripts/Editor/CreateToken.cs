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

    #region CreateAction
    [MenuItem("Worm Food/Create Action/Modify Worm Size")]
    static void CreateModifyWormSizeAction()
    {
        ModifyWormSize action = ScriptableObject.CreateInstance<ModifyWormSize>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\ModifyWormSize.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Worm Food/Create Action/Modify Worm Stomach Size")]
    static void CreateModifyWormStomachSizeAction()
    {
        ModifyWormStomachSize action = ScriptableObject.CreateInstance<ModifyWormStomachSize>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\ModifyWormStomachSize.asset");
        AssetDatabase.Refresh();
    }

    [MenuItem("Worm Food/Create Action/Drop Token")]
    static void CreateDropTokenAction()
    {
        DropToken action = ScriptableObject.CreateInstance<DropToken>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\DropToken.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Worm Food/Create Action/Multi Action")]
    static void CreateMultiAction()
    {
        MultiAction action = ScriptableObject.CreateInstance<MultiAction>();
        CreateDirectory(DefaultGameActionsFolder);
        AssetDatabase.CreateAsset(action, DefaultGameActionsFolder + @"\MultiAction.asset");
        AssetDatabase.Refresh();
    }
    #endregion

    #region CreateRecipe
    [MenuItem("Worm Food/Create Recipe")]
    static void CreateRecipe()
    {
        Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
        CreateDirectory(DefaultRecipesFolder);
        AssetDatabase.CreateAsset(recipe, DefaultRecipesFolder + @"\NewRecipe.asset");
        AssetDatabase.Refresh();
    }
    #endregion

    #region CreateCondition
    [MenuItem("Worm Food/Create Condition/Moves Taken")]
    static void CreateMovesTakenCondition()
    {
        MovesTakenCondition condition = ScriptableObject.CreateInstance<MovesTakenCondition>();
        CreateDirectory(DefaultConditionsFolder);
        AssetDatabase.CreateAsset(condition, DefaultConditionsFolder + @"\MovesTaken.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Worm Food/Create Condition/Resource")]
    static void CreateResourceCondition()
    {
        ResourceCondition condition = ScriptableObject.CreateInstance<ResourceCondition>();
        CreateDirectory(DefaultConditionsFolder);
        AssetDatabase.CreateAsset(condition, DefaultConditionsFolder + @"\ResourceCondition.asset");
        AssetDatabase.Refresh();
    }
    [MenuItem("Worm Food/Create Condition/Multi Condition")]
    static void CreateMultiCondition()
    {
        MultiCondition condition = ScriptableObject.CreateInstance<MultiCondition>();
        CreateDirectory(DefaultConditionsFolder);
        AssetDatabase.CreateAsset(condition, DefaultConditionsFolder + @"\MultiCondition.asset");
        AssetDatabase.Refresh();
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
