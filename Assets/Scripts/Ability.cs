using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Effect (GameObject obj, Ability a);
public delegate bool Condition (GameObject obj, Ability a);
public delegate string Description (Ability a);
public delegate bool Used (Ability a);
public delegate void Initializer (Ability a);

public enum AbilityType {
    Stun,
    Root,
    Move,
    Trap
}

public class Ability {
    private GameManager gameManager;
    private HexMap hexMap;
    public int maxDistance = 5;
    private int maxMove = 2;
    public int moveRemaining = 2;

    public bool used = false;
    public Effect effect;
    public Condition condition;
    public Description description = DefaultDescribe;
    public Used exhausted = DefaultExhaust;
    AbilityType type;

    static void InitStun (Ability a) {
        a.effect = Stun;
        a.condition = StunCondition;
    }

    static void InitMove (Ability a) {
        a.effect = Move;
        a.condition = InMoveRange;
        a.exhausted = Moves;
    }

    static void InitRoot (Ability a) {}

    static void InitTrap (Ability a) {
        a.effect = Trap;
        a.condition = InRange;
    }

    Dictionary<AbilityType, Initializer> AbilityInitializers = new Dictionary<AbilityType, Initializer> {
        {AbilityType.Stun, InitStun},
        {AbilityType.Move, InitMove},
        {AbilityType.Root, InitRoot},
        {AbilityType.Trap, InitTrap},
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
        gameManager.SubscribeToOnTileClick(Execute);
    }

    public void Turn () {
        used = false;
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
        int distance = tile.map.GetDistance(a.gameManager.playerPosition, tile);
        return distance <= a.moveRemaining;
    }

    static bool StunCondition (GameObject obj, Ability a) {
        return HasEnemy(obj, a) && InRange(obj, a);
    }

    static void Stun (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        var enemy = Enemy.EnemyLocations[tile];
        enemy.Stun();
    }

    static void Trap (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        var trap = GameObject.Instantiate(a.gameManager.trapPrefab, tile.transform.position, Quaternion.identity);
        tile.isTrapped = true;
    }

    static void Move (GameObject obj, Ability a) {
        Tile tile = obj.GetComponent<Tile>();
        int distance = tile.map.GetDistance(a.gameManager.playerPosition, tile);
        a.moveRemaining -= distance;
        a.gameManager.GetPlayer().transform.SetParent(tile.transform, false);
        a.gameManager.playerPosition = tile;
    }

    static bool DefaultExhaust (Ability a) {
        return true;
    }

    static bool Moves (Ability a) {
        return a.moveRemaining == 0;
    }

    static string DefaultDescribe (Ability a) {
        return a.type.ToString();
    }
}
