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
    public int rooted = 0;
    Renderer renderer;
    RangedAttack attack;
    [SerializeField] GameObject stunPrefab;
    GameObject stunImage;
    GameManager gameManager;

    public static Dictionary<Tile, Enemy> EnemyLocations = new Dictionary<Tile, Enemy>();

    private void Start () {
        map = GameObject.Find("HexMap").GetComponent<HexMap>();
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
        
        gameManager = Camera.main.GetComponent<GameManager>();
        gameManager.AddEnemy(this);
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
        transform.position = position.transform.position + new Vector3(0f, 1f, 0f);
    }

    float duration = .5f;
    static bool translating = false;
    IEnumerator Translate (Tile a, Tile b) {
        translating = true;
        Debug.Log("a.GetPosition()=" + a.GetPosition() + " b.GetPosition()=" + b.GetPosition());
        for (float t = 0f; t < 1f; t = t+Time.deltaTime/duration) {
            transform.position = Vector3.Lerp(a.GetPosition(), b.GetPosition(), t) + Vector3.up;
            yield return null;
        }
        SetPosition(b);

        sequence.Dequeue();
        if (sequence.Count > 0) {
            sequence.Peek()();
        }
        translating = false;
    }

    public delegate void Animation ();
    static Queue<Animation> sequence = new Queue<Animation>();
    void Move () {
        Tile tile = position;
        for (int i = 0; i < speed; i++) {
            var prevTile = tile;
            var nextTile = NextStep();
            if (nextTile is null)
                break;

            // SetPosition(tile);
            sequence.Enqueue(() => StartCoroutine(Translate(prevTile, nextTile)));
            tile = nextTile;
            if (!translating) {
                sequence.Peek()();
            }
            
            if (nextTile.isTrapped) // is trap
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
    
    public void Root() {
        rooted = 2;
        SetColor();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Turn();
        }
    }

    private void Turn () {
        //Wanted to separate these bc i didn't know how to cascade them right
        if (!stunned && rooted <= 0) {
            Move();
        }
        if (!stunned) {
            if (attack.CheckShot()) {
                attack.AttackPlayer();
            }
        }
        //I took away the else here bc for the same reason above
            stunned = false;
            rooted--;
        
        SetColor();
    }

    private void SetColor() {
        //// ALERT ////
        return; // ALERT short circuits
        //// ALERT ////

        //do any conditions supercede the other? stunned is stronger than rooted
        if (stunned)           {color = Color.yellow;}
        else if (rooted > 0 )  {color = Color.green; }
        else                   {color = Color.white; }
        renderer.material.color = color;
        Debug.Log("stunned:" + stunned + " rooted:" + rooted);
    }
    
}
