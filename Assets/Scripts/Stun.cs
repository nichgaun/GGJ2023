using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour {
    private GameManager gameManager;
    private HexMap hexMap;
    public int maxDistance = 5;
    
    private void Start () {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    //nick is cringe
    public void OnClick () {
        gameManager.SubscribeToOnTileClick(StunEnemy);
    }

    public void StunEnemy (GameObject obj) {
        Tile tile = obj.GetComponent<Tile>();

        int distance = tile.map.GetDistance(gameManager.playerPosition, tile);

        if (distance > maxDistance) {
            return;    
        }

        if (Enemy.EnemyLocations.ContainsKey(tile)) {
            var enemy = Enemy.EnemyLocations[tile];
            enemy.Stun();
        }

        gameManager.UnsubscribeToOnTileClick(StunEnemy);
    }

}
