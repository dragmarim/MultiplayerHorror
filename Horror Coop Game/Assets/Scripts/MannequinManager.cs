using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    public float delayBeforeActive;
    float currentTime = 0;
    public bool isActive;
    
    public GameObject mannequinIdle;
    public GameObject mannequinSitting;
    public GameObject couch;

    void Update() {
        currentTime += Time.deltaTime;
        if (currentTime >= delayBeforeActive && !isActive) {
            isActive = true;
            mannequinIdle.SetActive(false);
            mannequinSitting.SetActive(true);
            couch.transform.tag = "Rune";
        }
    }
}
