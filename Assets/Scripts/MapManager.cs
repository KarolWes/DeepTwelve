using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tilemap _map;
    [SerializeField] private Tile _floor;
    private List<Vector3Int> _notVisited;
    [SerializeField] private GameObject _wallTilePrefab, _startMarker, _endMarker;
    [SerializeField] private GameObject _wall3D, _marker3d;
    private Vector3Int _start, _end;

    public int[,] _neighbours = {{0, 1}, {1, 0}, {0, -1}, {-1, 0}};
    private Dictionary <Vector3Int, List<bool> > _walls;
    void Start() {
        _notVisited = new List<Vector3Int> ();
        _walls = new Dictionary<Vector3Int, List<bool>> ();
        Fill ();
        _start = _walls.Keys.ElementAt(Random.Range (0, _walls.Keys.Count));
        Generate(_start);
        SetUpWalls();
        GameObject ball = Instantiate(_marker3d, _start, Quaternion.Euler(new Vector3(0, 0, 0)));
        ball.transform.position = new Vector3 (_start.x, 0.5f, _start.y);
        ball.transform.position = ball.transform.position*5;
        _end = FindFurthest (_start);
        ball = Instantiate (_marker3d, _end, Quaternion.identity);
        var pos = ball.transform.position;
        ball.transform.position = new Vector3 (pos.x, 0.5f, pos.y);
        ball.transform.position = ball.transform.position*5;
        GameManager.Instance.UpdateGameState (GameState.Game);
    }

    void Fill() {
        for (int i = 0; i < _width; i ++)
        {
            for (int j = 0; j < _height; j ++)
            {
                var position = new Vector3Int (i+1, j+1, 0);
                _map.SetTile (position, _floor);
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
            var newPos = new Vector3Int (start.x + _neighbours[a, 0], start.y + _neighbours[a, 1], 0);
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
                while (_neighbours[i,0] != dir[0] || _neighbours[i,1] != dir[1])
                {
                    i ++;
                }
                _walls[prev][i] = false;
                _walls[act][(i + 2) % 4] = false;
                a = Random.Range (0, 3);
                for (int j = 0; j < 4; j ++)
                {
                    var newPos = new Vector3Int (act.x + _neighbours[a, 0], act.y + _neighbours[a, 1], 0);
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
                    GameObject wall = Instantiate(_wall3D, pos, Quaternion.Euler(new Vector3(0, 90*(i) , 0)));
                    wall.transform.position = new Vector3 (pos.x + 0.5f*_neighbours[i, 0]+0.5f, 0.5f, pos.y + 0.5f*_neighbours[i, 1]+0.5f);
                    wall.transform.position = wall.transform.position*5;
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
                var newPos = new Vector3Int (a.x + _neighbours[i, 0], a.y + _neighbours[i, 1], 0);
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
        return _width;
    }

    public int GetHeight() {
        return _height;
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
                    var next = new Vector3Int (act.x + _neighbours[i, 0], act.y + _neighbours[i, 1], 0);
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
