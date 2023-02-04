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

    public int GetDistance (Tile a, Tile b) {
        var ac = ToAxial(a);
        var bc = ToAxial(b);
        return AxialDistance(ac, bc);
    }

    public Tile GetTile (Vector2Int coordinate) {
        return map[coordinate.x][coordinate.y].GetComponent<Tile>();
    }

    public List<Tile> GetNeighbors (Tile tile) {
        List<Tile> tiles = new List<Tile>();

        return tiles;
    }
}
