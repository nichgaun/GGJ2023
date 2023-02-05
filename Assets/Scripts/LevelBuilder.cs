using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] List<Vector2Int> meleeBaddieCoords;
    [SerializeField] List<Vector2Int> rangedBaddieCoords;
    [SerializeField] List<Vector2Int> meleeMonsterCoords;

    [SerializeField] Vector2Int playerCoords;

    [SerializeField] List<Vector2Int> impassableTerrainCoords;
    [SerializeField] Vector2Int goalCoord;


    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject meleeEnemyPrefab;
    [SerializeField] GameObject rangedEnemyPrefab;
    [SerializeField] GameObject meleeMonsterPrefab;
    [SerializeField] GameObject impassableTerrainPrefab;
    [SerializeField] HexMap hexMap;
    [SerializeField] int nextSceneIndex;
    GameManager gameManager;


    public void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
        gameManager.nextSceneIndex = nextSceneIndex;
        GeneratePlayer(playerCoords);
        GenerateMeleeEnemies(meleeBaddieCoords);
        GenerateRangedEnemies(rangedBaddieCoords);
        GenerateMeleeMonsters(meleeMonsterCoords);
        GenerateImpassableTerrain(impassableTerrainCoords);
        GenerateGoal(goalCoord);
    }

    public void GenerateGoal (Vector2Int coord) {
        hexMap.SetupGoal(coord);
    }

    public void GeneratePlayer(Vector2Int coord) {
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<Player>().SetPosition(hexMap.GetTile(coord));
    }

    public void GenerateMeleeEnemies(List<Vector2Int> coords) {
        foreach (Vector2Int coord in coords) {
            GameObject enemy = Instantiate(meleeEnemyPrefab);
            enemy.GetComponent<Enemy>().SetPosition(hexMap.GetTile(coord));
        }
    }

    public void GenerateRangedEnemies(List<Vector2Int> coords) {
        foreach (Vector2Int coord in coords) {
            GameObject enemy = Instantiate(rangedEnemyPrefab);
            enemy.GetComponent<Enemy>().SetPosition(hexMap.GetTile(coord));
        }
    }

    public void GenerateMeleeMonsters(List<Vector2Int> coords) {
        foreach (Vector2Int coord in coords) {
            GameObject enemy = Instantiate(meleeMonsterPrefab);
            enemy.GetComponent<Enemy>().SetPosition(hexMap.GetTile(coord));
        }
    }  

    public void GenerateImpassableTerrain(List<Vector2Int> coords) {
        foreach (Vector2Int coord in coords) {
            Tile tile = hexMap.GetTile(coord);
            tile.passable = false;
            GameObject strawberry = Instantiate(impassableTerrainPrefab);
            strawberry.GetComponent<Strawberry>().SetPosition(tile);
        }
    }
}
