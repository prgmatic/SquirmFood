using UnityEngine;
using System.Collections.Generic;

public class ActiveRecipes : MonoBehaviour
{
    public static ActiveRecipes Instance { get { return _instance; } }
    private static ActiveRecipes _instance;

    public List<RecipeInActionOut> Recipes;

    /// <summary>
    /// Returns how many tokens were in the match so they can be removed from the worms stomach
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    public int CheckForMatches(Worm worm)
    {
        foreach(var recipe in Recipes)
        {
            if (recipe.Recipe.CheckForMatch(worm.Stomach))
            {
                if(recipe.Action != null)
                    recipe.Action.Execute(worm);
                return recipe.Recipe.Ingerdiants.Count;
            }
        }
        return 0;
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);
    }

	[System.Serializable]
    public class RecipeInActionOut
    {
        public Recipe Recipe;
        public GameAction Action;
    }
}
