using UnityEngine;
using System.Collections.Generic;

public class BoardLayout : ScriptableObject 
{
    public string GUID = "";
    [HideInInspector]
    public List<TokenAtPoint> Tokens = new List<TokenAtPoint>();
    [System.NonSerialized]
    public List<Playthrough> Playthroughs = new List<Playthrough>();

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
