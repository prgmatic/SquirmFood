using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Rotorz.ReorderableList;


[CustomEditor(typeof(ActiveRecipes))]
[CanEditMultipleObjects]
public class ActiveRecipesEditor : Editor
{
    const int spacing = 3;
    SerializedProperty _recipesProperty;

    void OnEnable()
    {
        _recipesProperty = serializedObject.FindProperty("Recipes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ReorderableListGUI.Title("Recipes");
        ReorderableListGUI.ListField(((ActiveRecipes)target).Recipes, RecipeActionDrawer, EditorGUIUtility.singleLineHeight * 2  + spacing * 3);
        serializedObject.ApplyModifiedProperties();
    }

    private ActiveRecipes.RecipeInActionOut RecipeActionDrawer(Rect position, ActiveRecipes.RecipeInActionOut itemValue)
    {
        position.y += spacing;
        position.height = EditorGUIUtility.singleLineHeight;
        itemValue.Recipe = (Recipe)EditorGUI.ObjectField(position, "Recipe", itemValue.Recipe, typeof(Recipe));
        position.y += EditorGUIUtility.singleLineHeight + spacing;
        itemValue.Action = (GameAction)EditorGUI.ObjectField(position, "Action", itemValue.Action, typeof(GameAction));

        return itemValue;
    }
}


