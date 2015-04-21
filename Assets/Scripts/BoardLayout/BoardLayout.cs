using UnityEngine;
using System.Collections.Generic;

public class BoardLayout : ScriptableObject 
{
    public int ID = -1;
    [HideInInspector]
    public List<TokenAtPoint> Tokens = new List<TokenAtPoint>();
    public Gameboard.BackgroundTileAttribute[] BackgroundTileAttributes;
    public int Columns;
    public int Rows;
    [System.NonSerialized]
    public List<Playthrough> Playthroughs = new List<Playthrough>();


	public bool IsValidCoordinate(int x, int y)
	{
		return x >= 0 && x < Columns && y >= 0 && y < Rows;
	}

	public Token GetTokenAt(int x, int y)
	{
		for(int i = 0;i < Tokens.Count; i++)
		{
			if(Tokens[i].Position.x == x && Tokens[i].Position.y == y)
			{
				return Tokens[i].Token;
			}
		}
		return null;
	}

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
