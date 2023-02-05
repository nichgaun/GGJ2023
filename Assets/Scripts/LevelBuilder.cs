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
    [SerializeField] Vector2Int dimensions;


    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject meleeEnemyPrefab;
    [SerializeField] GameObject rangedEnemyPrefab;
    [SerializeField] GameObject meleeMonsterPrefab;
    [SerializeField] GameObject impassableTerrainPrefab;
    [SerializeField] HexMap hexMap;
    [SerializeField] int nextSceneIndex;
    GameManager gameManager;

    [SerializeField] bool procedural = false;

    void ProcedurallyGenerateEnemies (GameObject prefab, int n) {
        for (int i = 0; i < n; i++) {
            var tile = hexMap.GetRandomTile();
            bool regen = true;
            while (regen) {
                tile = hexMap.GetRandomTile();
                regen = false;
                if (tile.passable == false || tile == hexMap.goalTile || tile == gameManager.playerPosition) {
                    regen = true;
                } else if (Enemy.EnemyLocations.ContainsKey(tile)) {
                    regen = true;
                }   
            }

            GameObject enemy = Instantiate(prefab);
            enemy.GetComponent<Enemy>().SetPosition(tile);
        }
    }


    public void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
        gameManager.nextSceneIndex = nextSceneIndex;

        if (procedural) {
            dimensions = new Vector2Int(20, 20);
            hexMap.GenerateHexMap(dimensions.x, dimensions.y);

            Vector2Int goalOffset = new Vector2Int(Random.Range(1,4), Random.Range(1,4));
            GenerateGoal(dimensions-goalOffset);
            GeneratePlayer(new Vector2Int(0, 0));

            int nTiles = dimensions.x*dimensions.y;
            int nImpass = (int) Random.Range(nTiles*0.10f, nTiles*.2f);
            for (int i = 0; i < nImpass; i++) {
                Tile tile = hexMap.GetRandomTile();

                bool regen = true;
                while (regen) {
                    tile = hexMap.GetRandomTile();
                    regen = false;
                    if (tile.passable == false || tile == hexMap.goalTile || tile == gameManager.playerPosition) {
                        regen = true;
                    } else {
                        tile.passable = false;
                        if (hexMap.FindPath(gameManager.playerPosition, hexMap.goalTile) == null) {
                            tile.passable = true;
                            regen = true;
                        }
                    }
                }
                GameObject strawberry = Instantiate(impassableTerrainPrefab);
                strawberry.GetComponent<Strawberry>().SetPosition(tile);
            }

            ProcedurallyGenerateEnemies(meleeEnemyPrefab, Random.Range(0,5));
            ProcedurallyGenerateEnemies(rangedEnemyPrefab, Random.Range(0,5));
            ProcedurallyGenerateEnemies(meleeMonsterPrefab, Random.Range(0,5));
        } else {
            GenerateMeleeEnemies(meleeBaddieCoords);
            GenerateRangedEnemies(rangedBaddieCoords);
            GenerateMeleeMonsters(meleeMonsterCoords);
            GenerateImpassableTerrain(impassableTerrainCoords);
            GenerateGoal(goalCoord);
        }
    }

    public void GenerateGoal (Vector2Int coord) {
        Debug.Log("hexMap=" + hexMap==null);
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
