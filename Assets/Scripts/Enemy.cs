using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// slows, roots, pushes

public class Enemy : MonoBehaviour {
    HexMap map;
    Tile position;
    List<Tile> path;
    int speed = 3;

    private void Start () {
        map = GameObject.Find("HexMap").GetComponent<HexMap>();
        map.AddEnemy(this);

        if (position == null) {
            SetPosition(map.GetRandomTile());
        }
    }

    public Tile NextStep () {
        Tile tile = null;
        if (path != null && path.Count > 0) {
            tile = (path[path.Count-1]);
            path.RemoveAt(path.Count-1);
        }
        return tile;
    }

    public void Seek (Tile tile) {
        path = map.FindPath(position, tile);
        NextStep(); // throw away first step
    }

    void SetPosition (Tile tile) {
        if (tile is null)
            return;

        position = tile;
        transform.position = position.transform.position + new Vector3(0f, 0f, 0.75f);
    }

    void Move () {
        Tile tile = null;
        for (int i = 0; i < speed; i++) {
            tile = NextStep();
            if (tile is null)
                break;

            SetPosition(tile);
            if (tile.isTrapped) // is trap
                break;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Move();
        }
    }
}
