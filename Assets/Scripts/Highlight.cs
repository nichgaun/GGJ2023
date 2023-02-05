using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public Tile position;
    public void SetPosition (Tile tile) {
        transform.position = tile.transform.position + Vector3.up;
    }
}
