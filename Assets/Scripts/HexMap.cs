using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexMap : MonoBehaviour {
    List<List<Tile>> map = new List<List<Tile>>();
    int width = 10, height = 10;
    public GameObject tilePrefab;
    float tileSize = 2f;
    float tileWidth, tileHeight; 
    [SerializeField] GameObject player;
    GameManager gameManager;

    private void Awake () {
        tileWidth = Mathf.Sqrt(3) * tileSize;
        tileHeight = 3/2 * tileSize;

        for (int i = 0; i < width; i++) {
            map.Add(new List<Tile>());
            for (int j = 0; j < height; j++) {
                float offset = j % 2 == 0 ? 0f : tileWidth/2f;

                var tile = Instantiate(
                    tilePrefab, 
                    new Vector3(i*tileWidth+offset, 0f, j*tileHeight*3f/2f), 
                    new Quaternion(0.707106829f,0,-0.707106829f,0), 
                    transform
                ).GetComponent<Tile>();

                tile.Initialize(i, j, this);
                map[i].Add(tile);
            }
        }

        Tile goalTile = GetTile(new Vector2Int(Random.Range(map.Count-3, map.Count), Random.Range(map.Count-3, map.Count)));
        goalTile.ChangeColor(Color.green);
        goalTile.isGoal = true;
    }

    public Tile GetRandomTile () {
        return GetTile(new Vector2Int (Random.Range((int)Mathf.Round(width/2), width), Random.Range((int)Mathf.Round(height/2), height)));
    }

    int AxialDistance (Vector2Int a, Vector2Int b) {
        return (Mathf.Abs(a.x - b.x) 
            + Mathf.Abs(a.x + a.y - b.x - b.y)
            + Mathf.Abs(a.y - b.y)) / 2;
    }

    Vector2Int ToAxial (Tile hex) {
        var q = hex.x - (hex.y - (hex.y&1)) / 2;
        var r = hex.y;
        return new Vector2Int (q, r);
    }

    Tile FromAxial (Vector2Int hex) {
        var col = hex.x + (hex.y - (hex.y&1)) / 2;
        var row = hex.y;
        return GetTile(new Vector2Int(col, row));
    }

    public int GetDistance (Tile a, Tile b) {
        var ac = ToAxial(a);
        var bc = ToAxial(b);
        return AxialDistance(ac, bc);
    }

    Vector3 CubeLerp (Vector3Int a, Vector3Int b, float t) {
        return new Vector3 (
            Mathf.Lerp(a.x, b.x, t),
            Mathf.Lerp(a.y, b.y, t),
            Mathf.Lerp(a.z, b.z, t)
        );
    }

    Vector3Int CubeRound (Vector3 frac) {
        int q = Mathf.RoundToInt(frac.x);
        int r = Mathf.RoundToInt(frac.y);
        int s = Mathf.RoundToInt(frac.z);

        var q_diff = Mathf.Abs(q - frac.x);
        var r_diff = Mathf.Abs(r - frac.y);
        var s_diff = Mathf.Abs(s - frac.z);

        if (q_diff > r_diff && q_diff > s_diff)
            q = -r-s;
        else if (r_diff > s_diff)
            r = -q-s;
        else
            s = -q-r;

        return new Vector3Int(q, r, s);
    }

    Vector2Int CubeToAxial (Vector3Int cube) {
        var q = cube.x;
        var r = cube.y;
        return new Vector2Int(q, r);
    }

    Vector3Int AxialToCube (Vector2Int hex) {
        var q = hex.x;
        var r = hex.y;
        var s = -q-r;
        return new Vector3Int(q, r, s);
    }

    Tile FromCube (Vector3Int cube) {
        return FromAxial(CubeToAxial(cube));
    }

    Vector3Int ToCube (Tile tile) {
        return AxialToCube(ToAxial(tile));
    }

    List<Tile> LineDraw (Tile a, Tile b) {
        int N = GetDistance(a, b);
        List<Tile> results = new List<Tile>();
        for (int i = 0; i < N; i++) {
            results.Add(FromCube(CubeRound(CubeLerp(ToCube(a), ToCube(b), 1f/(float)N*(float)i))));
        }
        return results;
    }

    public Tile GetTile (Vector2Int hex) {
        if (hex.x >= width || hex.x < 0 || hex.y >= height || hex.y < 0)
            return null;

        return map[hex.x][hex.y].GetComponent<Tile>();
    }

    List<Vector2Int> AxialDirections = new List<Vector2Int> {
        new Vector2Int (1, 0),
        new Vector2Int (1, -1),
        new Vector2Int (0, -1),
        new Vector2Int (-1, 0),
        new Vector2Int (-1, 1),
        new Vector2Int (0, 1),
    };

    public List<Tile> GetNeighbors (Tile tile) {
        List<Tile> tiles = new List<Tile>();

        var axial = ToAxial(tile);
        foreach (var direction in AxialDirections) {
            var neighbor = FromAxial(axial + direction);
            if (neighbor != null && neighbor.passable)
                tiles.Add(neighbor);
        }

        return tiles;
    }

    List<Tile> finders = new List<Tile>(), path = new List<Tile>();
    public void SetTileToPathFind (Tile tile) {
        foreach (var enemy in gameManager.enemies) {
            enemy.Seek(tile);
        }

        finders.Add(tile);
        if (finders.Count >= 2) {
            int c = finders.Count;
            // path = FindPath(finders[c-2], finders[c-1]);
            // path = LineDraw(finders[c-2], finders[c-1]);
            // Debug.Log("path.Count=" + path.Count);
            // foreach (Tile current in path) {
            //     Debug.Log("current="+current);
            //     current.ChangeColor(Color.red);
            // }
        }
    }

    List<Tile> GetPath (Dictionary<Tile, Tile> cameFrom, Tile current) {
        List<Tile> path = new List<Tile> {current};

        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Add(current);
        }

        return path;
    }

    private void OnDrawGizmos() {
        Tile current = null;
        Gizmos.color = Color.green;
        foreach (var tile in path) {
            if (current != null)
                Gizmos.DrawLine(current.transform.position, tile.transform.position);
            current = tile;
        }
    }

    private void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    public List<Tile> FindPath (Tile start, Tile goal) {
        Utils.PriorityQueue<Tile, float> openSet = new Utils.PriorityQueue<Tile, float> ();
        openSet.Enqueue(start, 0f);

        var cameFrom = new Dictionary<Tile, Tile>();
        var gScores = new Dictionary<Tile, float> { {start, 0f} };
        var fScores = new Dictionary<Tile, float> { {start, GetDistance(start, goal)} };

        while (openSet.Count != 0) {
            var current = openSet.Dequeue();

            if (current == goal) {
                return GetPath(cameFrom, current);
            }

            foreach (var neighbor in GetNeighbors(current)) {
                float gScore = gScores[current] + 1f;
                if (!gScores.ContainsKey(neighbor) || gScore < gScores[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScores[neighbor] = gScore;
                    fScores[neighbor] = gScore + GetDistance(neighbor, goal);
                    if (!openSet.UnorderedItems.Any(x => x.Element == neighbor))
                        openSet.Enqueue(neighbor, fScores[neighbor]);
                }
            }
        }
        return null;
    } 

    public bool LineOfSight(Tile position, Tile target) {
        List<Tile> path = FindPath(position, target);

        if (path.Count <= 2) {
            return true;
        }

        Vector2Int diff = ToAxial(path[0]) - ToAxial(path[1]);
        Debug.Log(diff);

        for (int i = 1; i < path.Count-1; i++) {
            Vector2Int newDiff = ToAxial(path[i]) - ToAxial(path[i+1]);
            Debug.Log(newDiff);
            if (newDiff != diff) {
                return false;
            }
        }

        return true;
       
    }   

    public List<Tile> GetTilesInRange(Tile tile, int range) {
        List<Tile> validTiles = new List<Tile>();
        validTiles.Add(tile);

        for (int i = 0; i<range; i++) {
            foreach (Tile option in validTiles) {
                List<Tile> tempList = GetNeighbors(option);
                validTiles = validTiles.Union(tempList).ToList();
            }
        }

        return validTiles;
    }
}
