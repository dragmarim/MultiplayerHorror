using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public GameObject player;
    public GameObject Key;
    public float woundUpTime;

    void FixedUpdate()
    {
        if (woundUpTime >= 10) {
            woundUpTime -= 0.1f;
            transform.Rotate(0, -1.8f, 0);
            transform.position += transform.forward * Time.deltaTime * 0.8f;
            Key.transform.Rotate(0, 0, -1);
        }
    }
}
