using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject hexMap;
    [SerializeField] GameObject player;

    private bool hasSubscriber;

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
        if (!hasSubscriber) {
            onClickCallback+=action;
            hasSubscriber = true;
        }    
    }

    public void UnsubscribeToOnTileClick(Action<GameObject> action) {
        int listSize = onClickCallback.GetInvocationList().Length;
        onClickCallback-=action;
        if (onClickCallback.GetInvocationList().Length < listSize) {
            hasSubscriber = false;
        }
        
    }

    public GameObject GetPlayer() {
        return player;
    }

}
