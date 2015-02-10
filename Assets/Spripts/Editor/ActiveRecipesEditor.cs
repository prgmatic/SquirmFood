using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using Rotorz.ReorderableList;


[CustomEditor(typeof(ActiveRecipes))]
[CanEditMultipleObjects]
public class ActiveRecipesEditor : Editor
{
    SerializedProperty _recipesProperty;

    void OnEnable()
    {
        _recipesProperty = serializedObject.FindProperty("Recipes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.IntField(1);
        //base.OnInspectorGUI();
        ReorderableListGUI.Title("Recipes");
        
        ReorderableListGUI.ListField(((ActiveRecipes)target).Recipes, PendingItemDrawer, EditorGUIUtility.singleLineHeight * 2  + 10);

        serializedObject.ApplyModifiedProperties();
    }

    private ActiveRecipes.RecipeInActionOut PendingItemDrawer(Rect position, ActiveRecipes.RecipeInActionOut itemValue)
    {
        // Text fields do not like null values!
        float labelWidth = 60;
        float width = position.width;

        position.y += 5;
        position.height = EditorGUIUtility.singleLineHeight;
        position.width = labelWidth;
        GUI.Label(position, "Recipe");
        position.x += labelWidth;
        position.width = width - labelWidth;
        itemValue.Recipe = (Recipe)EditorGUI.ObjectField(position, itemValue.Recipe, typeof(Recipe));
        position.x -= labelWidth;
        position.y += EditorGUIUtility.singleLineHeight;
        position.width = labelWidth;
        GUI.Label(position, "Action");
        position.x += labelWidth;
        position.width = width - labelWidth;
        itemValue.Action = (GameAction)EditorGUI.ObjectField(position, itemValue.Action, typeof(GameAction));

        //position.width -= 50;
        //EditorGUI.TextField(position, "hello");

        //position.x = position.xMax + 5;
        //position.width = 45;
        //if (GUI.Button(position, "Info"))
        //{
        //}

        return itemValue;
    }

    private void DrawEmpty()
    {
        GUILayout.Label("No items in list.", EditorStyles.miniLabel);
    }
}


