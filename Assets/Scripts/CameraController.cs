using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    float panSpeed = 10f;
    
    void Update () {
        transform.position += (Input.GetAxisRaw("Vertical")*Vector3.forward + Input.GetAxisRaw("Horizontal")*Vector3.right)*panSpeed*Time.deltaTime;
    }
}
