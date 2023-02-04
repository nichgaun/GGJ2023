using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public int maxRange = 3;
    public int damage = 10;
    private GameObject player;
    GameManager gameManager;
    HexMap hexMap;
    Enemy enemy;

    private void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
        hexMap = gameManager.hexMap.GetComponent<HexMap>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = gameObject.GetComponent<Enemy>();
    }

    public bool CheckShot() {
        string message = "checking shot ";
        if (hexMap.GetDistance(enemy.position, gameManager.playerPosition) > maxRange) {
            Debug.Log(message + "not in range");
            return false;
        }

        message += "in range";

        if( hexMap.LineOfSight(enemy.position, gameManager.playerPosition)) {
            Debug.Log(message + " have line of sight");
            return true;
        } else {
            Debug.Log(message + " no line of sight");
            return false;
        }
            
    }

    public void AttackPlayer() {
        player.GetComponent<Health>().Damage(damage);
    }
}
