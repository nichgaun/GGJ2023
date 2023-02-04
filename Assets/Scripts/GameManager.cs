using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject hexMap;
    [SerializeField] GameObject player;

    public Tile playerPosition;

    System.Action <GameObject> onClickCallback = (GameObject g)=>{};


    public void Start() {
        Tile startTile = hexMap.GetComponent<HexMap>().GetTile(new Vector2Int(0,0));
        player.transform.parent = startTile.transform;
        playerPosition = startTile;
    }

    public void ExecuteOnClick(GameObject obj) {
        onClickCallback(obj);
    }

    public void SubscribeToOnTileClick(Action<GameObject> action) {
        onClickCallback = (GameObject g)=>{};
        onClickCallback+=action;
    }

    public void UnsubscribeToOnTileClick(Action<GameObject> action) {
        onClickCallback-=action;
        
    }

    public GameObject GetPlayer() {
        return player;
    }

}
