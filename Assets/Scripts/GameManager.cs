using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject hexMap;
    [SerializeField] GameObject player;
    [SerializeField] public GameObject trapPrefab;
    [SerializeField] GameObject meleePrefab;
    [SerializeField] GameObject rangedPrefab;

    public Tile playerPosition;
    HexMap hexMapObj;

    System.Action <GameObject> onClickCallback = (GameObject g)=>{};    

    public List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy (Enemy e) {
        enemies.Add(e);
    }

    AbilityUI abilityUI;
    public void SetAbilityUI (AbilityUI aui) {
        abilityUI = aui;
    }

    public void Start() {
        hexMapObj = hexMap.GetComponent<HexMap>();
        Tile startTile = hexMapObj.GetTile(new Vector2Int(0,0));
        player.transform.parent = startTile.transform;
        playerPosition = startTile;
        GenerateEnemies(0, 3);
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

    public void Turn () {
        abilityUI.Turn();
        foreach(var enemy in enemies) {
            enemy.Turn();
        }
    }

    public void GenerateEnemies(int meleeEnemies = 2, int rangedEnemies = 2) {
        for (int i=0; i<meleeEnemies; i++) {
            var enemy = Instantiate(meleePrefab);
        }

        for (int i=0; i<rangedEnemies; i++) {
            var enemy = Instantiate(rangedPrefab);
        }
    }

}
