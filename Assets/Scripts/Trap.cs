using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private GameManager gameManager;
    private HexMap hexMap;
    public int maxDistance = 5;
    public GameObject trapPrefab;
    
    private void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    //nick is cringe
    public void OnClick(){
        gameManager.SubscribeToOnTileClick(PlaceTrap);
    }

    public void PlaceTrap(GameObject obj) {
        Tile tile = obj.GetComponent<Tile>();

        int distance = tile.map.GetDistance(gameManager.playerPosition, tile);

        if (distance > maxDistance) {
            return;    
        }

        var trap = Instantiate(trapPrefab, tile.transform.position, Quaternion.identity);
        tile.isTrapped = true;

        gameManager.UnsubscribeToOnTileClick(PlaceTrap);
    }

}
