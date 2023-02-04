using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int x, y;
    Renderer renderer;
    public bool selected = false, passable = true;
    Color color = Color.white;
    HexMap map;

    public void Initialize (int x, int y, HexMap map) {
        this.x = x;
        this.y = y;
        this.map = map;
    }

    public override string ToString () {
        return "x=" + x + " y=" + y;
    }

    // implement movement
    private void Awake() {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseOver() {
        selected = true;
        renderer.material.color = Color.cyan*color;
    }

    private void OnMouseExit() {
        selected = false;
        renderer.material.color = color;
    }

    private void OnMouseDown() {
        renderer.material.color = color;
        map.SetTileToPathFind(this);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1) && selected) {
            passable = !passable;
            color = passable ? Color.white : Color.gray;
            renderer.material.color = color;
        }
    }
}
