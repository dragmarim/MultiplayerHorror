using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    public GameObject player;

    public AudioClip poofIn;
    public AudioClip poofOut;

    public float delayBeforeActive;
    public float delayBeforeActiveMin;
    public float delayBeforeActiveMax;
    public float delayBeforeLeaving;
    public float delayBeforeLeavingMin;
    public float delayBeforeLeavingMax;
    float currentTime = 0;
    public bool isActive = false;
    public bool awayFromStand = false;
    public float maxIgnoreTime = 0;
    public GameObject blackout;
    
    public GameObject mannequinIdle;
    public GameObject mannequinAngry;
    public GameObject mannequinSitting;
    public GameObject mannequinPoint;
    public GameObject mannequinWantToPlay;
    public GameObject mannequinWantArcadeOff;
    public GameObject mannequinJumpscare;
    public GameObject couch;
    public GameObject car;
    public GameObject arcade;
    public bool isSitting;
    public bool isAngry;
    public bool waitingOnArcade = false;
    public AudioClip mannequinJumpscareClip;

    public GameObject particleCloud;

    bool moveTowardsPlayer = false;

    void Start() {
        delayBeforeLeaving = Random.Range(delayBeforeLeavingMin, delayBeforeLeavingMax);
    }

    void Update() {
        if (moveTowardsPlayer) {
            mannequinJumpscare.transform.position = Vector3.Lerp(mannequinJumpscare.transform.position, player.transform.position, currentTime / 40);
        }
        if (!player.GetComponent<BasicPlayerMovement>().isSitting) {
            currentTime += Time.deltaTime;
        }
        if (!awayFromStand && currentTime >= delayBeforeLeaving) {
            delayBeforeActive = Random.Range(delayBeforeActiveMin, delayBeforeActiveMax);
            AudioSource.PlayClipAtPoint(poofOut, mannequinIdle.transform.position, 0.1f);
            ParticleAppear(mannequinIdle);
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
                ParticleAppear(mannequinSitting);
                AudioSource.PlayClipAtPoint(poofIn, mannequinSitting.transform.position, 0.1f);
                player.GetComponent<BasicPlayerMovement>().timeSpentSitting = 0;
                isSitting = true;
                mannequinSitting.SetActive(true);
                couch.transform.tag = "Rune";
            } else if (rand == 2) {
                ParticleAppear(mannequinPoint);
                AudioSource.PlayClipAtPoint(poofIn, mannequinPoint.transform.position, 0.1f);
                mannequinPoint.SetActive(true);
                car.GetComponent<Drive>().success = false;
                car.transform.tag = "Rune";
            } else {
                ParticleAppear(mannequinWantToPlay);
                AudioSource.PlayClipAtPoint(poofIn, mannequinWantToPlay.transform.position, 0.1f);
                mannequinWantToPlay.SetActive(true);
                arcade.transform.tag = "Rune";
            }
        }
        if (isActive && currentTime >= maxIgnoreTime) {
            if (!car.GetComponent<Drive>().success && !player.GetComponent<BasicPlayerMovement>().isSitting && !waitingOnArcade) {
                if (!isAngry) {
                    ParticleAppear(mannequinAngry);
                    AudioSource.PlayClipAtPoint(poofOut, mannequinAngry.transform.position, 0.1f);
                    delayBeforeLeaving = Random.Range(delayBeforeLeavingMin, delayBeforeLeavingMax);
                    isAngry = true;
                    mannequinAngry.SetActive(true);
                    awayFromStand = false;
                    isActive = false;
                } else {
                    TriggerJumpscare();
                }
                currentTime = 0;
                if (couch.transform.tag == "Rune") {
                    ParticleAppear(mannequinSitting);
                    AudioSource.PlayClipAtPoint(poofOut, mannequinSitting.transform.position, 0.1f);
                    mannequinSitting.SetActive(false);
                    couch.transform.tag = "Untagged";
                } else if (car.transform.tag == "Rune") {
                    ParticleAppear(mannequinPoint);
                    AudioSource.PlayClipAtPoint(poofOut, mannequinPoint.transform.position, 0.1f);
                    mannequinPoint.SetActive(false);
                    car.transform.tag = "Untagged";
                } else {
                    ParticleAppear(mannequinWantToPlay);
                    AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.1f);
                    mannequinWantToPlay.SetActive(false);
                    arcade.GetComponent<Arcade>().ShutDown();
                    arcade.transform.tag = "Untagged";
                }
            }
        }
    }

    void ParticleAppear(GameObject target) {
        particleCloud.transform.position = target.transform.position;
        particleCloud.SetActive(false);
        particleCloud.SetActive(true);
    }

    public void TriggerJumpscare() {
        if (player.GetComponent<BasicPlayerMovement>().canBeJumpscared) {
            moveTowardsPlayer = true;
            player.GetComponent<BasicPlayerMovement>().DieFromMannequin();
            mannequinJumpscare.SetActive(true);
            mannequinJumpscare.transform.position = player.transform.position + player.transform.forward;
            mannequinJumpscare.transform.LookAt(player.transform.position);
            mannequinJumpscare.transform.eulerAngles = new Vector3(0, mannequinJumpscare.transform.eulerAngles.y, mannequinJumpscare.transform.eulerAngles.z);
            player.GetComponent<AudioSource>().clip = mannequinJumpscareClip;
            player.GetComponent<AudioSource>().Play();
            StartCoroutine(EndOfJumpscare());
            currentTime = 0;
        }
    }

    public void NoLongerSitting() {
        delayBeforeLeaving = Random.Range(delayBeforeLeavingMin, delayBeforeLeavingMax);
        ParticleAppear(mannequinSitting);
        AudioSource.PlayClipAtPoint(poofOut, mannequinSitting.transform.position, 0.1f);
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
        delayBeforeLeaving = Random.Range(delayBeforeLeavingMin, delayBeforeLeavingMax);
        ParticleAppear(mannequinPoint);
        AudioSource.PlayClipAtPoint(poofOut, mannequinPoint.transform.position, 0.1f);
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
        ParticleAppear(mannequinWantToPlay);
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.1f);
        mannequinWantToPlay.SetActive(false);
    }

    public void DemandTurnArcadeOff() {
        currentTime = 0;
        waitingOnArcade = false;
        ParticleAppear(mannequinWantArcadeOff);
        AudioSource.PlayClipAtPoint(poofIn, mannequinWantArcadeOff.transform.position, 0.1f);
        mannequinWantArcadeOff.SetActive(true);
    }

    public void DoneWithArcade() {
        delayBeforeLeaving = Random.Range(delayBeforeLeavingMin, delayBeforeLeavingMax);
        arcade.transform.tag = "Untagged";
        ParticleAppear(mannequinWantArcadeOff);
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantArcadeOff.transform.position, 0.1f);
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
        delayBeforeLeaving = Random.Range(delayBeforeLeavingMin, delayBeforeLeavingMax);
        waitingOnArcade = false;
        if (!isAngry) {
            isAngry = true;
            mannequinAngry.SetActive(true);
        } else {
            TriggerJumpscare();
        }
        arcade.transform.tag = "Untagged";
        ParticleAppear(mannequinWantToPlay);
        AudioSource.PlayClipAtPoint(poofOut, mannequinWantToPlay.transform.position, 0.1f);
        mannequinWantToPlay.SetActive(false);
        isActive = false;
        awayFromStand = false;
        currentTime = 0;
    }

    IEnumerator EndOfJumpscare() {
        yield return new WaitForSeconds(0.9f);
        blackout.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<AudioSource>().Stop();
    }
}
