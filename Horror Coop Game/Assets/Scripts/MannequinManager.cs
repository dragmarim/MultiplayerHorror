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
    public float maxIgnoreTime = 0;
    
    public GameObject mannequinIdle;
    public GameObject mannequinAngry;
    public GameObject mannequinSitting;
    public GameObject mannequinPoint;
    public GameObject mannequinWantToPlay;
    public GameObject mannequinWantArcadeOff;
    public GameObject couch;
    public GameObject car;
    public GameObject arcade;
    public bool isSitting;
    public bool isAngry;
    public bool waitingOnArcade = false;

    void Update() {
        if (!player.GetComponent<BasicPlayerMovement>().isSitting) {
            currentTime += Time.deltaTime;
        }
        if (!awayFromStand && currentTime >= delayBeforeLeaving) {
            AudioSource.PlayClipAtPoint(poofOut, mannequinIdle.transform.position, 0.3f);
            awayFromStand = true;
            currentTime = 0;
            mannequinIdle.SetActive(false);
            mannequinAngry.SetActive(false);
        }
        if (awayFromStand && currentTime >= delayBeforeActive && !isActive) {
            currentTime = 0;
            isActive = true;
            Random.InitState((int)System.DateTime.Now.Ticks);
            int rand = Random.Range(1, 4);
            if (rand == 1) {
                AudioSource.PlayClipAtPoint(poofIn, mannequinSitting.transform.position, 0.3f);
                player.GetComponent<BasicPlayerMovement>().timeSpentSitting = 0;
                isSitting = true;
                mannequinSitting.SetActive(true);
                couch.transform.tag = "Rune";
            } else if (rand == 2) {
                AudioSource.PlayClipAtPoint(poofIn, mannequinPoint.transform.position, 0.3f);
                mannequinPoint.SetActive(true);
                car.GetComponent<Drive>().success = false;
                car.transform.tag = "Rune";
            } else {
                AudioSource.PlayClipAtPoint(poofIn, mannequinWantToPlay.transform.position, 0.3f);
                mannequinWantToPlay.SetActive(true);
                arcade.transform.tag = "Rune";
            }
        }
        if (isActive && currentTime >= maxIgnoreTime) {
            if (!isAngry && !car.GetComponent<Drive>().success && !player.GetComponent<BasicPlayerMovement>().isSitting && !waitingOnArcade) {
                isAngry = true;
                isActive = false;
                mannequinAngry.SetActive(true);
                currentTime = 0;
                awayFromStand = false;
                if (couch.transform.tag == "Rune") {
                    AudioSource.PlayClipAtPoint(poofOut, mannequinSitting.transform.position, 0.3f);
                    mannequinSitting.SetActive(false);
                    couch.transform.tag = "Untagged";
                } else if (car.transform.tag == "Rune") {
                    AudioSource.PlayClipAtPoint(poofOut, mannequinPoint.transform.position, 0.3f);
                    mannequinPoint.SetActive(false);
                    car.transform.tag = "Untagged";
                } else {
                    AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.3f);
                    mannequinWantToPlay.SetActive(false);
                    arcade.GetComponent<Arcade>().ShutDown();
                    arcade.transform.tag = "Untagged";
                }
            } else if (!car.GetComponent<Drive>().success && !player.GetComponent<BasicPlayerMovement>().isSitting && !waitingOnArcade) {

            }
        }
    }

    public void NoLongerSitting() {
        AudioSource.PlayClipAtPoint(poofOut, mannequinSitting.transform.position, 0.3f);
        couch.transform.tag = "Untagged";
        isSitting = false;
        mannequinSitting.SetActive(false);
        if (isAngry) {
            mannequinAngry.SetActive(true);
        } else {
            mannequinIdle.SetActive(true);
        }
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void CarFinished() {
        AudioSource.PlayClipAtPoint(poofOut, mannequinPoint.transform.position, 0.3f);
        mannequinPoint.SetActive(false);
        if (isAngry) {
            mannequinAngry.SetActive(true);
        } else {
            mannequinIdle.SetActive(true);
        }
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void TurnedOnArcade() {
        currentTime = 0;
        waitingOnArcade = true;
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.3f);
        mannequinWantToPlay.SetActive(false);
    }

    public void DemandTurnArcadeOff() {
        currentTime = 0;
        waitingOnArcade = false;
        AudioSource.PlayClipAtPoint(poofIn, mannequinWantArcadeOff.transform.position, 0.3f);
        mannequinWantArcadeOff.SetActive(true);
    }

    public void DoneWithArcade() {
        arcade.transform.tag = "Untagged";
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantArcadeOff.transform.position, 0.3f);
        mannequinWantArcadeOff.SetActive(false);
        if (isAngry) {
            mannequinAngry.SetActive(true);
        } else {
            mannequinIdle.SetActive(true);
        }
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    public void FailedArcade() {
        if (!isAngry) {
            isAngry = true;
            mannequinAngry.SetActive(true);
        } else {
            
        }
        arcade.transform.tag = "Untagged";
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.3f);
        mannequinWantToPlay.SetActive(false);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }
}
