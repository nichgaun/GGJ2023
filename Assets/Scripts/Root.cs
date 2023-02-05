using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {
    //Copied from Stun
    private GameManager gameManager;
    private HexMap hexMap;
    public int maxDistance = 5;
    public bool used = false;
    
    private void Start () {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    //nick is cringe
    public void OnClick () {
        gameManager.SubscribeToOnTileClick(RootEnemy);
    }

    public void RootEnemy (GameObject obj) {
        Tile tile = obj.GetComponent<Tile>();

        int distance = tile.map.GetDistance(gameManager.playerPosition, tile);

        if (distance > maxDistance) {
            return;    
        }

        if (Enemy.EnemyLocations.ContainsKey(tile)) {
            var enemy = Enemy.EnemyLocations[tile];
            enemy.Root();
            used = true;
        }

        gameManager.UnsubscribeToOnTileClick(RootEnemy);
    }

}
