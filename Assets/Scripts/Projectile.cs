using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed;
    Vector3 target;

    public void Init(float _speed, Vector3 _target)
    {
        speed = _speed;
        target = _target;
    }

    void Update()
    {
        Vector3 diff = target - transform.position;

        Debug.Log("DIFF: " + diff + " [" + diff.magnitude + "]");

        if (diff.magnitude < speed * Time.deltaTime)
        {
            Destroy(gameObject);
        }

        transform.position = transform.position + diff.normalized * speed * Time.deltaTime;
    }
}
