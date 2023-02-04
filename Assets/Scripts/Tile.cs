using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int x, y;
    Renderer renderer;

    public void Initialize (int x, int y) {
        this.x = x;
        this.y = y;
    }

    // implement movement
    private void Awake() {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseOver() {
        renderer.material.color = Color.cyan;
        Debug.Log("moused over x=" + x + " y= " + y);
    }
}
