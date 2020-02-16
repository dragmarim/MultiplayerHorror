using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject button;
    bool isSwitched;
    
    void Start() {
        isSwitched = false;
    }

    void OnMouseDown() {
        if (isSwitched) {
            button.transform.position = new Vector3(0, 0, 0);
        }
    }
}
