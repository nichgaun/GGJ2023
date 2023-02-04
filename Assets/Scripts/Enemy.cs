using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    HexMap map;
    Tile position;
    List<Tile> path;

    private void Start () {
        map = GameObject.Find("HexMap").GetComponent<HexMap>();
        map.AddEnemy(this);

        if (position == null) {
            SetPosition(map.GetRandomTile());
        }
    }

    public void Seek (Tile tile) {
        path = map.FindPath(position, tile);
    }

    void SetPosition (Tile tile) {
        position = tile;
        transform.position = position.transform.position + new Vector3(0f, 0f, 0.75f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (path != null && path.Count > 0) {
                SetPosition(path[path.Count-1]);
                path.RemoveAt(path.Count-1);
            }
        }
    }
}
