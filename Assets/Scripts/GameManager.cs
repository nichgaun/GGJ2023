using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] public GameObject hexMap;
    GameObject player;
    [SerializeField] public GameObject trapPrefab;
    [SerializeField] GameObject meleePrefab;
    [SerializeField] GameObject rangedPrefab;
    [SerializeField] GameObject meleeMonsterPrefab;

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
        Tile startTile = hexMapObj.GetTile(new Vector2Int(0,0));
        player.GetComponent<Player>().SetPosition(startTile);
        GenerateEnemies(1, 1, 1);
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

    public void GenerateEnemies(int meleeEnemies = 2, int rangedEnemies = 2, int meleeMonster = 1) {
        for (int i=0; i<meleeEnemies; i++) {
            var enemy = Instantiate(meleePrefab);
        }

        for (int i=0; i<rangedEnemies; i++) {
            var enemy = Instantiate(rangedPrefab);
        }

        for (int i=0; i<meleeMonster; i++) {
            var enemy = Instantiate(meleeMonsterPrefab);
        }
    }

}
