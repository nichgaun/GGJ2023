using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] List<Vector2> meleeBaddieCoords;
    [SerializeField] List<Vector2> rangedBaddieCoords;
    [SerializeField] List<Vector2> meleeMonsterCoords;

    [SerializeField] Vector2Int playerCoords;

    [SerializeField] List<Vector2> impassableTerrainCoords;


    [SerializeField] GameObject playerPrefab;
    [SerializeField] HexMap hexMap;


    public void Start() {
        GeneratePlayer(playerCoords);
    }

    public void GeneratePlayer(Vector2Int coords) {
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<Player>().SetPosition(hexMap.GetTile(coords));
    }
}
