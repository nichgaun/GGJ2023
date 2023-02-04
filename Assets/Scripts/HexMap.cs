using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {
    List<List<GameObject>> map = new List<List<GameObject>>();
    int width = 10, height = 10;
    public GameObject tilePrefab;
    float tileSize = 2f;
    float tileWidth, tileHeigth; 

    private void Awake() {
        tileWidth = Mathf.Sqrt(3) * tileSize;
        tileHeigth = 3/2 * tileSize;
    }

    private void Start() {
        for (int i = 0; i < width; i++) {
            map.Add(new List<GameObject>());
            for (int j = 0; j < height; j++) {
                float offset = j % 2 == 0 ? 0f : tileWidth/2f;
                var tile = Instantiate(tilePrefab, new Vector3(i*tileWidth+offset, j*tileHeigth*3f/2f, 0), new Quaternion(0.5f,-0.5f,-0.5f,0.5f), transform);
                tile.GetComponent<Tile>().x = i;
                tile.GetComponent<Tile>().y = j;
                map[i].Add(tile);
                
            }
        }
    }
}
