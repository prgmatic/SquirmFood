using UnityEngine;
using System.Collections.Generic;

public class Recipe : ScriptableObject 
{
    public bool MatchAnyOrder = true;
    public List<Token> Ingerdiants;

    public bool CheckForMatch(List<Token> tokens)
    {
        if (tokens.Count < Ingerdiants.Count)
            return false;

        for(int i = 0; i < Ingerdiants.Count; i++)
        {
            if(Ingerdiants[i] != null && Ingerdiants[i].name != tokens[tokens.Count - Ingerdiants.Count + i].name)
            {
                if (!MatchAnyOrder) return false;
                break;
            }
        }
        if (!MatchAnyOrder) return true;
        for(int i = Ingerdiants.Count - 1; i >= 0; i--)
        {
            if(Ingerdiants[i] != null && Ingerdiants[i].name != tokens[tokens.Count - 1 - i].name)
            {
                return false;
            }
        }

        return true;
    }
}
