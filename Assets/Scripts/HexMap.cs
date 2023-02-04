using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {
    List<List<Tile>> map = new List<List<Tile>>();
    int width = 10, height = 10;
    public GameObject tilePrefab;
    float tileSize = 2f;
    float tileWidth, tileHeigth; 

    private void Awake () {
        tileWidth = Mathf.Sqrt(3) * tileSize;
        tileHeigth = 3/2 * tileSize;
    }

    private void Start () {
        for (int i = 0; i < width; i++) {
            map.Add(new List<Tile>());
            for (int j = 0; j < height; j++) {
                float offset = j % 2 == 0 ? 0f : tileWidth/2f;

                var tile = Instantiate(
                    tilePrefab, 
                    new Vector3(i*tileWidth+offset, j*tileHeigth*3f/2f, 0), 
                    new Quaternion(0.5f,-0.5f,-0.5f,0.5f), 
                    transform
                ).GetComponent<Tile>();

                tile.Initialize(i, j);
                map[i].Add(tile);
            }
        }
    }

    int AxialDistance (Vector2Int a, Vector2Int b) {
        return (Mathf.Abs(a.x - b.x) 
            + Mathf.Abs(a.x + a.y - b.x - b.y)
            + Mathf.Abs(a.y - b.y)) / 2;
    }

    Vector2Int ToAxial (Tile hex) {
        var q = hex.y - (hex.x - (hex.y&1)) / 2;
        var r = hex.x;
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
            if (tile != null)
                tiles.Add(neighbor);
        }

        return tiles;
    }
}
