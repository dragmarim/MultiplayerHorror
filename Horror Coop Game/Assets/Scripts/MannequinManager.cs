using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    public GameObject player;

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
    public bool isSitting;

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
            int rand = 1;
            if (rand == 1) {
                player.GetComponent<BasicPlayerMovement>().timeSpentSitting = 0;
                isSitting = true;
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

    public void NoLongerSitting() {
        couch.transform.tag = "Untagged";
        isSitting = false;
        mannequinSitting.SetActive(false);
        mannequinIdle.SetActive(true);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void CarFinished() {
        mannequinPoint.SetActive(false);
        mannequinIdle.SetActive(true);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }
}
