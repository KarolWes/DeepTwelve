using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [FormerlySerializedAs ("_width")] [SerializeField] private int width;
    [FormerlySerializedAs ("_height")] [SerializeField] private int height;
    [FormerlySerializedAs ("_map")] [SerializeField] private Tilemap map;
    [FormerlySerializedAs ("_floor")] [SerializeField] private Tile floor;
    //[SerializeField] private GameObject _wallTilePrefab, _startMarker, _endMarker;
    [FormerlySerializedAs ("_wall3D")] [SerializeField] private GameObject wall3D;
    private Vector3Int _start, _end;
    private List<Vector3Int> _notVisited;
    public const int Scale = 10;

    public readonly int[,] Neighbours = {{0, 1}, {1, 0}, {0, -1}, {-1, 0}};
    private Dictionary <Vector3Int, List<bool> > _walls;
    void Start() {
        _notVisited = new List<Vector3Int> ();
        _walls = new Dictionary<Vector3Int, List<bool>> ();
        Fill ();
        _start = _walls.Keys.ElementAt(Random.Range (0, _walls.Keys.Count));
        Generate(_start);
        SetUpWalls();
        _end = FindFurthest (_start);
        Debug.Log(_end);
        GameManager.Instance.UpdateGameState (GameState.Game);
    }

    void Fill() {
        for (int i = 0; i < width; i ++)
        {
            for (int j = 0; j < height; j ++)
            {
                var position = new Vector3Int (i+1, j+1, 0);
                map.SetTile (position, floor);
                map.size = new Vector3Int (Scale, Scale, Scale); // poprawić
                _notVisited.Add (position);
                _walls.Add (position, new List<bool> {true, true, true, true});
            }
        }
    }

    void Generate(Vector3Int start) {
        var q = new Stack< Tuple <Vector3Int, Vector3Int>> ();
        int a = Random.Range (0, 3);
        var goal = start;
        for (int i = 0; i < 4; i ++)
        {
            var newPos = new Vector3Int (start.x + Neighbours[a, 0], start.y + Neighbours[a, 1], 0);
            q.Push (new Tuple<Vector3Int, Vector3Int> (start, newPos));
            a = (a + 1) % 4;
        }
        _notVisited.Remove (start);
        while (q.Count > 0)
        {
            var tmp = q.Pop();
            var prev = tmp.Item1;
            var act = tmp.Item2;
            if (_notVisited.Contains (act))
            {
                goal = act;
                _notVisited.Remove (act);
                int[] dir = {act.x - prev.x, act.y-prev.y};
                int i = 0;
                while (Neighbours[i,0] != dir[0] || Neighbours[i,1] != dir[1])
                {
                    i ++;
                }
                _walls[prev][i] = false;
                _walls[act][(i + 2) % 4] = false;
                a = Random.Range (0, 3);
                for (int j = 0; j < 4; j ++)
                {
                    var newPos = new Vector3Int (act.x + Neighbours[a, 0], act.y + Neighbours[a, 1], 0);
                    q.Push (new Tuple<Vector3Int, Vector3Int> (act, newPos));
                    a = (a + 1) % 4;
                }
            }
        }
    }

    void SetUpWalls() {
        foreach (var pos in _walls.Keys)
        {
            for(int i = 0; i < 4; i++)
            {
                var w = _walls[pos][i];
                if (w)
                {
                    GameObject wall = Instantiate(wall3D, pos, Quaternion.Euler(new Vector3(0, 90*(i) , 0)));
                    wall.transform.localScale = new Vector3 (Scale, Scale, 0.1f);
                    wall.transform.position = new Vector3 (pos.x + 0.5f*Neighbours[i, 0]+0.5f, 0.5f, pos.y + 0.5f*Neighbours[i, 1]+0.5f);
                    wall.transform.position = wall.transform.position*Scale;
                }
            }
        }
    }

    public Vector3Int FindFurthest(Vector3Int start) {
        var q = new Queue<Vector3Int> ();
        var furthest = start;
        var visited = new List<Vector3Int> ();
        q.Enqueue (start);
        while (q.Count > 0)
        {
            var a = q.Dequeue ();
            visited.Add (a);
            furthest = a;
            for (int i = 0; i < 4; i ++)
            {
                var newPos = new Vector3Int (a.x + Neighbours[i, 0], a.y + Neighbours[i, 1], 0);
                if (!_walls[a][i])
                {
                    if (!visited.Contains (newPos))
                    {
                        q.Enqueue (newPos);
                    }
                }
            }
        }
        return furthest;
    }

    public List<bool> GetAccessible(Vector3Int tile) {
        return _walls[tile];
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public Vector3Int GetStartPoint() {
        return _start;
    }

    public Vector3Int GetEndPoint() {
        return _end;
    }

    public List<Vector3Int> Escape(Vector3Int start) {
        var path = new Dictionary<Vector3Int, Vector3Int> ();
        var q = new Queue<Vector3Int> ();
        q.Enqueue (start);
        while (q.Count > 0)
        {
            var act = q.Dequeue ();
            if (act == _end)
            {
                break;
            }

            var neigh = GetAccessible (act);
            for (var i = 0; i < 4; i ++)
            {
                if (!neigh[i])
                {
                    var next = new Vector3Int (act.x + Neighbours[i, 0], act.y + Neighbours[i, 1], 0);
                    if (!path.ContainsKey (next))
                    {
                        path.Add (next, act);
                        q.Enqueue (next);
                    }
                }
            }
        }

        var steps = new List<Vector3Int> ();
        steps.Add (_end);
        var pos = _end;
        while (pos != start)
        {
            pos = path[pos];
            steps.Add (pos);
        }

        steps.Reverse ();
        return steps;
    } 
}
