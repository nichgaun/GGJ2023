using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void Effect (GameObject obj, Ability a);
public delegate bool Condition (GameObject obj, Ability a);
public delegate string Description (Ability a);
public delegate bool Used (Ability a);
public delegate void Initializer (Ability a);
public delegate void Highlighter (Ability a);

public enum AbilityType {
    Stun,
    Root,
    Move,
    Trap,
    Slow,
}

public class Ability {
    private GameManager gameManager;
    private HexMap hexMap;
    public int maxDistance = 5;
    private int maxMove = 2;
    public int moveRemaining = 2;
    public int cooldown = 0, cooldownMax = 0;
    private bool needClick = true;

    public bool used = false;
    public Effect effect = DefaultEffect;
    public Condition condition = DefaultCondition;
    public Description description = DefaultDescribe;
    public Used exhausted = DefaultExhaust;
    public Highlighter highlighter = DefaultHighlighter;
    AbilityType type;

    static void InitStun (Ability a) {
        a.effect = Stun;
        a.condition = StunCondition;
        a.cooldownMax = 3;
        a.highlighter = TargetHighlighter;
        a.description = CooldownDescribe;
    }

    static void InitMove (Ability a) {
        a.effect = Move;
        a.condition = InMoveRange;
        a.exhausted = Moves;
        a.highlighter = MoveHighlighter;
    }

    static void InitRoot (Ability a) {
        a.effect = Root;
        a.condition = RootCondition;
        a.cooldownMax = 2;
        a.highlighter = TargetHighlighter;
    }

    static void InitSlow (Ability a) {
        a.effect = Slow;
        a.condition = RootCondition;
        a.cooldownMax = 1;
        a.highlighter = TargetHighlighter;
    }

    static void InitTrap (Ability a) {
        a.effect = Trap;
        a.condition = InRange;
        a.cooldownMax = 3;
        a.highlighter = TrapHighlighter;
    }

    Dictionary<AbilityType, Initializer> AbilityInitializers = new Dictionary<AbilityType, Initializer> {
        {AbilityType.Stun, InitStun},
        {AbilityType.Move, InitMove},
        {AbilityType.Root, InitRoot},
        {AbilityType.Trap, InitTrap},
        {AbilityType.Slow, InitSlow},
    };

    public void Initialize (AbilityType type) {
        this.type = type;
        AbilityInitializers[type](this);
        gameManager = Camera.main.GetComponent<GameManager>();
    }
    
    private void Start () {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    //nick is based
    public void OnClick () {
        if (needClick) {
            highlighter(this);
            gameManager.SubscribeToOnTileClick(Execute);
        } else {
            Execute(null);
        }
    }

    public void Turn () {
        cooldown--;
        if (cooldown <= 0) {
            used = false;
        }
        moveRemaining = maxMove;
    }

    public void Execute (GameObject obj) {
        if (condition(obj, this)) {
            effect(obj, this);
            used = exhausted(this);
        }

        gameManager.UnsubscribeToOnTileClick(Execute);
    }

    static bool HasEnemy (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        return Enemy.EnemyLocations.ContainsKey(tile);
    }

    static bool InRange (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        int distance = tile.map.GetDistance(a.gameManager.playerPosition, tile);
        return distance <= a.maxDistance;
    }

    static bool InMoveRange (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        int distance = tile.map.FindPath(a.gameManager.playerPosition, tile).Count-1;

        if (tile.Occupied())
            return false;

        // Debug.Log("distance="+distance);
        return distance <= a.moveRemaining;
    }

    static bool StunCondition (GameObject obj, Ability a) {
        return HasEnemy(obj, a) && InRange(obj, a);
    }

    static bool RootCondition (GameObject obj, Ability a) {
        return HasEnemy(obj, a) && InRange(obj, a);
    }

    static bool DefaultCondition (GameObject obj, Ability a) {
        return true;
    }
    
    static void DefaultEffect (GameObject obj, Ability a) {
        Debug.Log("default!");
    }

    static void Stun (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        var enemy = Enemy.EnemyLocations[tile];
        enemy.Stun();
    }

    static void Root (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        var enemy = Enemy.EnemyLocations[tile];
        enemy.Root();
    }

    static void Slow (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        var enemy = Enemy.EnemyLocations[tile];
        enemy.Slow();
    }

    static void Trap (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        var trap = GameObject.Instantiate(a.gameManager.trapPrefab, tile.transform.position, Quaternion.identity);
        tile.isTrapped = true;
    }

    static void Move (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        int distance = tile.map.FindPath(a.gameManager.playerPosition, tile).Count-1;
        a.moveRemaining -= distance;

        var path = a.gameManager.hexMapObj.FindPath(a.gameManager.playerPosition, tile);

        for (int i = path.Count-1; i > 0; i--) {
            var player = a.gameManager.GetPlayer();
            a.gameManager.QueueTranslation(player, path[i], path[i-1]);
        }

        if (tile.isGoal) {
            //Display complete screen
            //-1 for victory screen
            if (a.gameManager.nextSceneIndex != -1) {
                SceneManager.LoadScene(a.gameManager.nextSceneIndex);
            } else {
                //set to -1 for last screen
                //go to last screen index
            }
        }
    }

    void goalEntered() {
        
    }
    static bool DefaultExhaust (Ability a) {
        a.cooldown = a.cooldownMax;
        return true;
    }

    static bool Moves (Ability a) {
        return a.moveRemaining <= 0;
    }

    static string CooldownDescribe (Ability a) {
        return a.type.ToString() + " CD:" + a.cooldown.ToString();
    }

    static string DefaultDescribe (Ability a) {
        return a.type.ToString();
    }

    static void DefaultHighlighter (Ability a) {
        //nothing
    }

    static void MoveHighlighter(Ability a) {
        Tile tile = a.gameManager.playerPosition;
        List<Tile> moveTiles = tile.map.GetTilesInRange(tile, a.moveRemaining);

        foreach (Tile validTile in moveTiles) {
            validTile.hexHighlight.SetActive(true);
        }
    }

    static void TargetHighlighter(Ability a) {
        Tile tile = a.gameManager.playerPosition;
        List<Tile> moveTiles = tile.map.GetTilesInRange(tile, a.maxDistance);

        foreach (Tile validTile in moveTiles) {
            if (validTile.Occupied() && validTile != tile) {
                validTile.hexHighlight.SetActive(true);
            }
            
        }
    }

    static void TrapHighlighter(Ability a) {
        Tile tile = a.gameManager.playerPosition;
        List<Tile> moveTiles = tile.map.GetTilesInRange(tile, a.maxDistance);

        foreach (Tile validTile in moveTiles) {
            if (validTile != tile) {
                validTile.hexHighlight.SetActive(true);
            }
            
        }
    }
}
