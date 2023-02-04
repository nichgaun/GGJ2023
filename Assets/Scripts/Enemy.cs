using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// slows, roots, pushes

public class Enemy : MonoBehaviour {
    HexMap map;
    public Tile position;
    List<Tile> path;
    int speed = 3;
    Color color = Color.white;
    public bool stunned = false;
    Renderer renderer;
    RangedAttack attack;
    [SerializeField] GameObject stunPrefab;
    GameObject stunImage;

    public static Dictionary<Tile, Enemy> EnemyLocations = new Dictionary<Tile, Enemy>();

    private void Start () {
        map = GameObject.Find("HexMap").GetComponent<HexMap>();
        map.AddEnemy(this);
        renderer = GetComponent<Renderer>();
        attack = gameObject.GetComponent<RangedAttack>();

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();

        stunImage = Instantiate(stunPrefab);
        stunImage.transform.SetParent(canvas.transform, false);
        stunImage.transform.localScale = new Vector3(3.5f,3.5f,3.5f);
        stunImage.SetActive(false);

        if (position == null) {
            SetPosition(map.GetRandomTile());
        }
    }

    public Tile NextStep () {
        Tile tile = null;
        if (path != null && path.Count > 0) {
            tile = (path[path.Count-1]);
            path.RemoveAt(path.Count-1);
        }
        return tile;
    }

    public void Seek (Tile tile) {
        path = map.FindPath(position, tile);
        NextStep(); // throw away first step
    }

    void SetPosition (Tile tile) {
        if (tile is null)
            return;
        
        if (position != null && EnemyLocations.ContainsKey(position))
            EnemyLocations.Remove(position);
        position = tile;
        EnemyLocations[position] = this;
        transform.position = position.transform.position + new Vector3(0f, 0f, 0.75f);
    }

    void Move () {
        Tile tile = null;
        for (int i = 0; i < speed; i++) {
            tile = NextStep();
            if (tile is null)
                break;

            SetPosition(tile);
            if (tile.isTrapped) // is trap
                break;
        }
    }

    public void Stun() {
        stunned = true;
        SetColor();

        StartCoroutine(ShowStun());
    }

    IEnumerator ShowStun() {
        stunImage.SetActive(true);
        yield return new WaitForSeconds(1);
        stunImage.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Turn();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Stun();
        }
    }

    private void Turn () {
        if (!stunned) {
            Move();
            if (attack.CheckShot()) {
                attack.AttackPlayer();
            }
        }
        else {
            stunned = false;
        }
        SetColor();
    }

    private void SetColor() {
        color = stunned ? Color.yellow : Color.white;
        renderer.material.color = color;
    }
    
}
