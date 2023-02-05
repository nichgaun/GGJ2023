using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] List<Vector2> meleeBaddieCoords;
    [SerializeField] List<Vector2> rangedBaddieCoords;
    [SerializeField] List<Vector2> meleeMonsterCoords;

    [SerializeField] Vector2 playerCoords;

    [SerializeField] List<Vector2> impassableTerrainCoords;

    public void Start() {

    }

    public void GeneratePlayer(Vector2 coordss) {

    }
}
