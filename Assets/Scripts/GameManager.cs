using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] public GameObject hexMap;
    [SerializeField] GameObject player;
    [SerializeField] public GameObject trapPrefab;
    [SerializeField] GameObject meleePrefab;
    [SerializeField] GameObject rangedPrefab;

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
        hexMapObj = hexMap.GetComponent<HexMap>();
        Tile startTile = hexMapObj.GetTile(new Vector2Int(0,0));
        player.GetComponent<Player>().SetPosition(startTile);
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

    public delegate void Animation ();
    static Queue<Animation> sequence = new Queue<Animation>();
    public void QueueTranslation (GameObject g, Tile a, Tile b) {
        sequence.Enqueue(() => StartCoroutine(GameManager.Translate(g, a, b)));
        if (!translating) {
            sequence.Peek()();
        }
    }

    static float duration = .5f;
    static bool translating = false;
    static public IEnumerator Translate (GameObject g, Tile a, Tile b) {
        translating = true;
        g.transform.SetParent(null);
        // Debug.Log("name=" + g.name);
        // Debug.Log("a.GetPosition()=" + a.GetPosition() + " b.GetPosition()=" + b.GetPosition());
        for (float t = 0f; t < 1f; t = t+Time.deltaTime/duration) {
            float y = g.transform.position.y;
            g.transform.position = Vector3.Lerp(a.GetPosition(), b.GetPosition(), t) + Vector3.up*y;
            yield return null;
        }

        if (g.GetComponent<Enemy>() != null)
            g.GetComponent<Enemy>().SetPosition(b);
        else if (g.GetComponent<Player>() != null) {
            g.GetComponent<Player>().SetPosition(b);
        }

        sequence.Dequeue();
        if (sequence.Count > 0) {
            sequence.Peek()();
        }
        translating = false;
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
