using UnityEngine;
using System.Collections.Generic;

public class BoardLayout : ScriptableObject 
{
    [HideInInspector]
    public List<TokenAtPoint> Tokens = new List<TokenAtPoint>();

    [System.Serializable]
    public class TokenAtPoint
    {
        public Token Token;
        public Point Position;

        public TokenAtPoint(Token token, Point position)
        {
            this.Token = token;
            this.Position = position;
        }
    }
}
