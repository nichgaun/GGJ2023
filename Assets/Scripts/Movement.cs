using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private GameManager gameManager;
    private HexMap hexMap;
    private int maxMove = 2;
    private int moveRemaining = 2;

    private void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
        hexMap = GameObject.Find("HexMap").GetComponent<HexMap>();
    }

    //nick is cringe
    public void OnClick(){
        gameManager.SubscribeToOnTileClick(Move);
    }

    public void Move(GameObject obj) {
        Tile tile = obj.GetComponent<Tile>();

        Debug.Log("Moving to: "+obj.name);

        int distance = hexMap.GetDistance(gameManager.playerPosition, tile);

        Debug.Log(distance);

        if (distance > moveRemaining) {
            return;    
        }

        moveRemaining -= distance;

        gameManager.GetPlayer().transform.SetParent(tile.transform, false);

    }
}
