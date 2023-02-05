using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// slows, roots, pushes

public class Enemy : MonoBehaviour {
    HexMap map;
    public Tile position, target;
    List<Tile> path;
    int speed = 3, maxSpeed = 3;
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

    public void Seek (Tile tile) {
        target = tile;
    }

    public void SetPosition (Tile tile) {
        if (tile is null)
            return;

        if (EnemyLocations.ContainsKey(tile) && EnemyLocations[tile] != this) {
            Debug.LogError("what the flip");
        }
        
        if (position != null && EnemyLocations.ContainsKey(position))
            EnemyLocations.Remove(position);

        position = tile;
        EnemyLocations[position] = this;


        // foreach (var t in EnemyLocations.Keys) {
        //     if (EnemyLocations[t].position != t) {
        //         Debug.LogError("oh no! EnemyLocations[t].position=" + EnemyLocations[t].position + " vs t=" + t);
        //     }
        // }

        transform.position = position.transform.position + Vector3.up;
    }

    delegate void Mover ();
    static Queue<Mover> moves = new Queue<Mover>();
    static public bool moving = false;
    
    IEnumerator Move () {
        moving = true;
        target = gameManager.playerPosition;

        var path = gameManager.hexMapObj.FindPath(position, target);

        if (path != null) {
            path.Reverse();


            for (int i = 0; i < path.Count-1 && i < speed; i++) {
                if (gameObject.GetComponent<RangedAttack>() != null && gameObject.GetComponent<RangedAttack>().CheckShot()) {
                    Debug.Log("hey");
                    break;
                }

                if (path[i+1].Occupied())
                    break;

                yield return GameManager.Translate(gameObject, path[i], path[i+1]);
            }
        }

        if (moves.Count > 0) {
            moves.Dequeue()();
        } else {
            moving = false;
        }
    }

    void QueueMove () {
        moves.Enqueue(() => StartCoroutine(Move()));
        if (!moving) {
            moves.Dequeue()();
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
    public void Slow() {
        speed = 1;
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     Turn();
        // }
    }

    public void Turn () {
        //Wanted to separate these bc i didn't know how to cascade them right
        if (!stunned && rooted <= 0) {
            QueueMove();
        }
        if (!stunned) {
            if (attack.CheckShot()) {
                attack.AttackPlayer();
            }
        }
        stunned = false;
        rooted--;
        if (speed < maxSpeed) {
            speed++;
        }
        
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
    }
    
}
