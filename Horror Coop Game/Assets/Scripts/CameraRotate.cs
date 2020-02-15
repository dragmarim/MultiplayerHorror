using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 10;
    public float currentRotation;

    void Start() {
        currentRotation = rotationSpeed;
    }

    void Update() {
        transform.rotation = Quaternion.Euler(30, currentRotation, 0);
        currentRotation += rotationSpeed;
        if (currentRotation > 360) {
            currentRotation -= 360;
        }
    }
}
