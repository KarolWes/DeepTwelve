using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Random = UnityEngine.Random;

public class generate : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tilemap _map;
    [SerializeField] private Tile _floor;
    private List<Vector3Int> _notVisited;
    [SerializeField] private GameObject _wallTilePrefab;
    
    private int[,] _neighbours = {{0, 1}, {1, 0}, {0, -1}, {-1, 0}};
    private Dictionary <Vector3Int, List<bool> > _walls;
    void Start() {
        _notVisited = new List<Vector3Int> ();
        _walls = new Dictionary<Vector3Int, List<bool>> ();
        Fill ();
        
        var start = _walls.Keys.ElementAt(Random.Range (0, _walls.Keys.Count));
        Generate(start);
        SetUpWalls();
    }
    
    void Fill() {
        for (int i = 0; i < _width; i ++)
        {
            for (int j = 0; j < _height; j ++)
            {
                var position = new Vector3Int (i, j, 0);
                _map.SetTile (position, _floor);
                _notVisited.Add (position);
                _walls.Add (position, new List<bool> {true, true, true, true});
            }
        }
    }

    void Generate(Vector3Int start) {
        var q = new Stack<Vector3Int> ();
        q.Push (start);
        _notVisited.Remove (start);
        while (q.Count > 0)
        {
            var act = q.Pop();
            int a = Random.Range (0, 3);
            for (int i = 0; i < 4; i ++)
            {
                var newPos = new Vector3Int (act.x + _neighbours[a, 0], act.y + _neighbours[a, 1], 0);
                if (_notVisited.Contains (newPos))
                {
                    _walls[act][a] = false;
                    _walls[newPos][(a + 2) % 4] = false;
                    q.Push (newPos);
                    _notVisited.Remove (newPos);
                }
                a = (a + 1) % 4;
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
                    GameObject wall = Instantiate(_wallTilePrefab, pos, Quaternion.Euler(new Vector3(0, 0, 90*i)));
                    wall.transform.position = new Vector3 (pos.x + 0.5f*_neighbours[i, 0], pos.y + 0.5f*_neighbours[i, 1], 0);
                }
            }
        }
    }
    
}
