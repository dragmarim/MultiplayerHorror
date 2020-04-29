using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    public float delayBeforeActive;
    public float delayBeforeLeaving;
    float currentTime = 0;
    public bool isActive = false;
    public bool awayFromStand = false;
    
    public GameObject mannequinIdle;
    public GameObject mannequinSitting;
    public GameObject mannequinPoint;
    public GameObject mannequinWantToPlay;
    public GameObject couch;
    public GameObject car;
    public GameObject arcade;

    void Update() {
        currentTime += Time.deltaTime;
        if (!awayFromStand && currentTime >= delayBeforeLeaving) {
            awayFromStand = true;
            currentTime = 0;
            mannequinIdle.SetActive(false);
        }
        if (awayFromStand && currentTime >= delayBeforeActive && !isActive) {
            isActive = true;
            //int rand = Random.Range(1, 4);
            int rand = 2;
            if (rand == 1) {
                mannequinSitting.SetActive(true);
                couch.transform.tag = "Rune";
            } else if (rand == 2) {
                mannequinPoint.SetActive(true);
                car.transform.tag = "Rune";
            } else {
                mannequinWantToPlay.SetActive(true);
                arcade.transform.tag = "Rune";
            }
        }
    }
}
