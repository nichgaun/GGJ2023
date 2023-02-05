using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    float panSpeed = 40f, zoomSpeed = 500f;
    
    void Update () {
        transform.position += (Input.GetAxisRaw("Vertical")*Vector3.forward + Input.GetAxisRaw("Horizontal")*Vector3.right)*panSpeed*Time.deltaTime;
        transform.position += transform.forward * Input.mouseScrollDelta.y * Time.deltaTime*zoomSpeed;
    }
}
