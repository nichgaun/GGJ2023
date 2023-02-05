using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameManager gameManager;
    // Start is called before the first frame update
    void Awake () {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    public void SetPosition (Tile tile) {
        transform.position = tile.GetPosition() + 2*Vector3.up;
        gameManager.playerPosition = tile;
    }
}
