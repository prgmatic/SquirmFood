using UnityEngine;
using System.Collections.Generic;

public class BoardInitalizer : MonoBehaviour 
{
    public List<TokenWithWeight> TokensToSpawn;

    void Start()
    {
        FillGameboard();
    }

    private void FillGameboard()
    {
        if (CheckIfTokensToSpawnAreNull()) return;
        for(int y = 0; y < Gameboard.Instance.Rows; y++)
        {
            for(int x = 0; x< Gameboard.Instance.Columns; x++)
            {
                int index = GetTokenIndexFromWeightedValues();
                Gameboard.Instance.AddTileFromToken(TokensToSpawn[index].Token, x, y, false);
            }
        }
    }

    private bool CheckIfTokensToSpawnAreNull()
    {
        foreach (var token in TokensToSpawn)
        {
            if(token.Token == null)
            {
                Debug.LogError("There can no null tokens in the board initializer");
                return true;
            }
        }
        return false;
    }

    private int GetTokenIndexFromWeightedValues()
    {
        int randMax = 0;
        foreach (var token in TokensToSpawn)
        {
            randMax += token.SpawnWeight;
        }
        int rand = Random.Range(0, randMax);

        int weightValue = 0;
        for (int i = 0; i < TokensToSpawn.Count; i++)
        {
            TokenWithWeight token = TokensToSpawn[i];
            if (rand >= weightValue && rand < weightValue + token.SpawnWeight)
            {
                return i;
            }
            else
            {
                weightValue += token.SpawnWeight;
            }
        }
        return -1;
    }

    [System.Serializable]
    public class TokenWithWeight
    {
        public Token Token;
        [Range(0, 100)]
        public int SpawnWeight = 50;
    }
}
