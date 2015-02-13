using UnityEngine;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour 
{

    private List<Vector3> pathLine = new List<Vector3>();
    private int validPaths = 0;

    void Awake()
    {
        DebugHUD.MessagesCleared += DebugHUD_MessagesCleared;
        Gameboard.Instance.TileAdded += delegate { FindPath(); };
        Gameboard.Instance.TileDestroyed += delegate { FindPath(); };
        Gameboard.Instance.TileSettled += delegate { FindPath(); };
    }

    private void DebugHUD_MessagesCleared(object sender, System.EventArgs e)
    {
        DebugHUD.Add(string.Format("Valid Paths: {0}", validPaths));
    }

    void Update()
    {
        if (pathLine.Count > 1)
        {
            for(int i = 0;i < pathLine.Count - 1; i++)
            {
                DrawArrow(pathLine[i], pathLine[i + 1], Color.red);
            }
        }
        //Vector2 pos = new Vector2(-0.5f, -0.5f);
        //DrawArrow(pos, new Vector2(0.5f, -0.5f), Color.green);
        
    }

    public void FindPath()
    {
        if (Gameboard.Instance.GameState != Gameboard.GameStateType.InProgress || !enabled) return;
        pathLine.Clear();
        validPaths = 0;
        Worm worm = null;

        foreach (var tile in Gameboard.Instance.gameTiles)
        {
            Worm w = tile.GetComponent<Worm>();
            if (w != null)
            {
                worm = w;
                break;
            }
        }

        if(worm != null)
        {
            int edibleTiles = 0;
            foreach(var tile in Gameboard.Instance.gameTiles)
            {
                if (tile.IsEdible) edibleTiles++;
            }

            Path p = new Path();
            p.Tiles.Add(worm.Head);
            p.StartBranchOut();
            foreach (var path in Path.AllPaths)
            {
                if(path.Tiles.Count == edibleTiles + 1)
                {
                    validPaths++;
                    if(pathLine.Count == 0)
                    {
                        foreach(var tile in path.Tiles)
                        {
                            pathLine.Add(tile.WorldPosition);
                        }
                    }
                }
            }
        }
    }
	
    private void DrawArrow(Vector3 pos, Direction direction, Color color)
    {
        float angle = 0;
        switch(direction)
        {
            case Direction.Left:
                angle = 90;
                break;
            case Direction.Right:
                angle = -90;
                break;
            case Direction.Down:
                angle = 180;
                break;
        }
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        float edgeLength = 0.35f;
        Vector3 tip = pos + rotation * Vector3.up;
        Debug.DrawLine(pos, tip, color);
        Debug.DrawLine(tip, tip + rotation * new Vector3(edgeLength, -edgeLength), color);
        Debug.DrawLine(tip, tip + rotation * new Vector3(-edgeLength, -edgeLength), color);
    }

    public void DrawArrow(Vector3 start, Vector3 tip, Color color)
    {
        float edgeLength = 0.3f;
        Quaternion rotation = Utils.QuaternionLookAt(start, tip) * Quaternion.Euler(0,0,-90);
        Debug.DrawLine(start, tip, color);
        Vector3 halfwayPoint = start + (tip - start) / 2;

        Debug.DrawLine(halfwayPoint, halfwayPoint + rotation * new Vector3(edgeLength, -edgeLength), color);
        Debug.DrawLine(halfwayPoint, halfwayPoint + rotation * new Vector3(-edgeLength, -edgeLength), color);

    }

    public class Path
    {
        public static List<Path> AllPaths = new List<Path>();
        public int NeighborsAtEnd = 0;
        public List<GameTile> Tiles = new List<GameTile>();

        public Path()
        {
            AllPaths.Add(this);

        }

        public Path(List<GameTile> tiles)
        {
            this.Tiles = tiles;
            AllPaths.Add(this);
        }

        public void StartBranchOut()
        {
            AllPaths.Clear();
            BranchOut();
        }

        public void BranchOut()
        {
            var neighbors = Gameboard.Instance.GetCardinalNeighbors(Tiles[Tiles.Count - 1]);
            foreach(var neighbor in neighbors)
            {
                if(neighbor.IsEdible && !Tiles.Contains(neighbor))
                {
                    Path p = new Path();
                    p.Tiles.AddRange(Tiles.ToArray());
                    p.Tiles.Add(neighbor);
                    p.BranchOut();
                }
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
