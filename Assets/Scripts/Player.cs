using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameManager gameManager;
    // Start is called before the first frame update
    void Awake () {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    public void DoneMoving () {
        Debug.Log("done moving");
        if (gameManager.playerPosition.isGoal) {
            //Display complete screen
            //-1 for victory screen
            if (gameManager.nextSceneIndex != -1) {
                gameManager.ShowVictory();
            } else {
                //set to -1 for last screen
                //go to last screen index
            }
        }
    }

    public void SetPosition (Tile tile) {
        transform.position = tile.GetPosition() + 2*Vector3.up;
        gameManager.playerPosition = tile;
    }
}
