using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int x, y;
    Renderer renderer;
    bool selected = false;
    private GameManager gameManager;

    public void Initialize (int x, int y) {
        this.x = x;
        this.y = y;
    }

    private void Start() {
        gameManager = Camera.main.GetComponent<GameManager>();
    }

    // implement movement
    private void Awake() {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseOver() {
        selected = true;
        renderer.material.color = Color.cyan;
    }

    private void OnMouseExit() {
        selected = false;
        renderer.material.color = Color.white;
    }

    private void OnMouseUpAsButton() {
        gameManager.ExecuteOnClick(gameObject);
    }
}
