using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    public GameObject player;

    public AudioClip poofIn;
    public AudioClip poofOut;

    public float delayBeforeActive;
    public float delayBeforeLeaving;
    float currentTime = 0;
    public bool isActive = false;
    public bool awayFromStand = false;
    
    public GameObject mannequinIdle;
    public GameObject mannequinSitting;
    public GameObject mannequinPoint;
    public GameObject mannequinWantToPlay;
    public GameObject mannequinWantArcadeOff;
    public GameObject couch;
    public GameObject car;
    public GameObject arcade;
    public bool isSitting;

    void Update() {
        currentTime += Time.deltaTime;
        if (!awayFromStand && currentTime >= delayBeforeLeaving) {
            AudioSource.PlayClipAtPoint(poofOut, mannequinIdle.transform.position, 0.3f);
            awayFromStand = true;
            currentTime = 0;
            mannequinIdle.SetActive(false);
        }
        if (awayFromStand && currentTime >= delayBeforeActive && !isActive) {
            isActive = true;
            //int rand = Random.Range(1, 4);
            int rand = 3;
            if (rand == 1) {
                AudioSource.PlayClipAtPoint(poofIn, mannequinSitting.transform.position, 0.3f);
                player.GetComponent<BasicPlayerMovement>().timeSpentSitting = 0;
                isSitting = true;
                mannequinSitting.SetActive(true);
                couch.transform.tag = "Rune";
            } else if (rand == 2) {
                AudioSource.PlayClipAtPoint(poofIn, mannequinPoint.transform.position, 0.3f);
                mannequinPoint.SetActive(true);
                car.transform.tag = "Rune";
            } else {
                AudioSource.PlayClipAtPoint(poofIn, mannequinWantToPlay.transform.position, 0.3f);
                mannequinWantToPlay.SetActive(true);
                arcade.transform.tag = "Rune";
            }
        }
    }

    public void NoLongerSitting() {
        AudioSource.PlayClipAtPoint(poofOut, mannequinSitting.transform.position, 0.3f);
        couch.transform.tag = "Untagged";
        isSitting = false;
        mannequinSitting.SetActive(false);
        mannequinIdle.SetActive(true);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void CarFinished() {
        AudioSource.PlayClipAtPoint(poofOut, mannequinPoint.transform.position, 0.3f);
        mannequinPoint.SetActive(false);
        mannequinIdle.SetActive(true);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void TurnedOnArcade() {
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.3f);
        mannequinWantToPlay.SetActive(false);
    }

    public void DemandTurnArcadeOff() {
        AudioSource.PlayClipAtPoint(poofIn, mannequinWantArcadeOff.transform.position, 0.3f);
        mannequinWantArcadeOff.SetActive(true);
    }

    public void DoneWithArcade() {
        arcade.transform.tag = "Untagged";
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantArcadeOff.transform.position, 0.3f);
        mannequinWantArcadeOff.SetActive(false);
        mannequinIdle.SetActive(true);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void FailedArcade() {
        arcade.transform.tag = "Untagged";
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.3f);
        mannequinWantToPlay.SetActive(false);
        mannequinIdle.SetActive(true);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }
}
