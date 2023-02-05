using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] public GameObject hexMap;
    GameObject player;
    [SerializeField] public GameObject trapPrefab;

    public Tile playerPosition;
    public HexMap hexMapObj;

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
        player = GameObject.FindGameObjectWithTag("Player");
        hexMapObj = hexMap.GetComponent<HexMap>();
    }

    public void ExecuteOnClick(GameObject obj) {
        onClickCallback(obj);
        ClearTileHighlights();
    }

    public void SubscribeToOnTileClick(Action<GameObject> action) {
        onClickCallback = (GameObject g)=>{};
        onClickCallback+=action;
    }

    public void UnsubscribeToOnTileClick(Action<GameObject> action) {
        onClickCallback-=action;
        
    }

    public void ClearTileHighlights() {
        foreach (Tile tile in hexMapObj.allTiles) {
            tile.hexHighlight.SetActive(false);
        }
    }

    public delegate void Animation ();
    static Queue<Animation> sequence = new Queue<Animation>();
    public void QueueTranslation (GameObject g, Tile a, Tile b) {
        sequence.Enqueue(() => StartCoroutine(GameManager.Translate(g, a, b)));
        if (!translating) {
            sequence.Dequeue()();
        }
    }

    static void SetPosition (GameObject g, Tile tile) {
        if (g.GetComponent<Enemy>() != null)
            g.GetComponent<Enemy>().SetPosition(tile);
        else if (g.GetComponent<Player>() != null) {
            g.GetComponent<Player>().SetPosition(tile);
        }
    }

    static float duration = .5f;
    static bool translating = false;
    static public IEnumerator Translate (GameObject g, Tile a, Tile b) {
        translating = true;

        for (float t = 0f; t < 1f; t = t+Time.deltaTime/duration) {
            float y = g.transform.position.y;
            g.transform.position = Vector3.Lerp(a.GetPosition(), b.GetPosition(), t) + Vector3.up*y;
            yield return null;
        }

        // Debug.Log("b");
        SetPosition(g, b);

        if (sequence.Count > 0) {
            sequence.Dequeue()();
        } else {
            translating = false;
        }
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

}
