using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCooldown : MonoBehaviour
{
    public int jumpscareWaitMin;
    public int jumpscareWaitMax;
    public GameObject jumpscareSpider;
    public bool canFall;
    public bool startFalling;
    public GameObject blackout;

    public bool bangOnVentOnCooldown;
    public GameObject player;
    public GameObject ventLight;

    public GameObject spider;
    public bool spiderIsVisible;
    public bool waitForFlicker;
    public bool spiderLeftVent;
    public bool waited;

    public int nothingInVentTimerDefault;
    int nothingInVentTimer;
    public int spiderApproachingVentTimerDefault;
    int spiderApproachingVentTimer;
    public int timeToUnscrew;
    int timeLeftUntilUnscrew;
    public int remainingScrews;

    public AudioClip bangOnVent;
    public AudioClip spiderWalking;
    public AudioClip spiderWalking2;
    public AudioClip screwFalling;
    public AudioClip ventCoverFallOff;
    AudioSource aud;

    void Start() {
        startFalling = false;
        canFall = false;
        bangOnVentOnCooldown = false;
        waited = false;
        waitForFlicker = false;
        spiderLeftVent = false;
        spiderIsVisible = false;
        aud = GetComponent<AudioSource>();
        nothingInVentTimer = nothingInVentTimerDefault;
        spiderApproachingVentTimer = spiderApproachingVentTimerDefault;
        StartCoroutine(Countdown());
    }

    void Update() {
        if (canFall) {
            if (!startFalling) {
                jumpscareSpider.GetComponent<AudioSource>().Play();
                startFalling = true;
                jumpscareSpider.transform.position = new Vector3(player.transform.position.x, 5, player.transform.position.z);
                jumpscareSpider.transform.position += player.transform.forward / 3.6f;
                jumpscareSpider.transform.eulerAngles = new Vector3(70, player.transform.eulerAngles.y-180, 0);
                player.GetComponent<BasicPlayerMovement>().DieFromSpider();
            }
            if (jumpscareSpider.transform.position.y > 2.15f) {
                jumpscareSpider.transform.position = new Vector3 (jumpscareSpider.transform.position.x, jumpscareSpider.transform.position.y-0.1f, jumpscareSpider.transform.position.z);
            } else {
                blackout.SetActive(true);
            }
        }
    }

    IEnumerator Countdown() {
        if (waitForFlicker && !ventLight.GetComponent<Flicker>().lightIsOff) {
            waitForFlicker = false;
            waited = true;
        }
        yield return new WaitForSeconds(1);
        if (nothingInVentTimer > 0) {
            if (spiderIsVisible && ventLight.GetComponent<Flicker>().lightIsOff) {
                spiderIsVisible = false;
                spider.SetActive(false);
                Debug.Log("spider invisible");
            }
            nothingInVentTimer -= 1;
            if (nothingInVentTimer == 0) {
                //aud.clip = spiderWalking;
                int rand = Random.Range(1, 3);
                if (rand == 1) {
                    AudioSource.PlayClipAtPoint(spiderWalking, transform.position, 0.5f);
                } else {
                    AudioSource.PlayClipAtPoint(spiderWalking2, transform.position, 0.5f);
                }
            }
        } else if (spiderApproachingVentTimer > 0) {
            spiderApproachingVentTimer -= 1;
            if (spiderApproachingVentTimer == 0) {
                timeLeftUntilUnscrew = timeToUnscrew;
            }
        } else {
            if (!spiderIsVisible && ventLight.GetComponent<Flicker>().lightIsOff && !spiderLeftVent) {
                spiderIsVisible = true;
                spider.SetActive(true);
                Debug.Log("spider now visible");
            }
            timeLeftUntilUnscrew -= 1;
            if (timeLeftUntilUnscrew <= 0 && ventLight.GetComponent<Flicker>().lightIsOff && !spiderLeftVent) {
                //aud.clip = screwFalling;
                if (!waitForFlicker && !waited) {
                    AudioSource.PlayClipAtPoint(screwFalling, transform.position, 0.2f);
                }
                remainingScrews -= 1;
                if (remainingScrews > 0) {
                    timeLeftUntilUnscrew = timeToUnscrew;
                } else if (!waited) {
                    waitForFlicker = true;
                } else {
                    spiderLeftVent = true;
                    spiderIsVisible = false;
                    spider.SetActive(false);
                    this.tag = ("Untagged");
                    yield return new WaitForSeconds(2);
                    //GetComponent<Rigidbody>().isKinematic = false;
                    //GetComponent<Rigidbody>().useGravity = true;
                    transform.eulerAngles = new Vector3(40, -135, -135);
                    AudioSource.PlayClipAtPoint(ventCoverFallOff, transform.position, 0.8f);
                    yield return new WaitForSeconds(1);
                    GetComponent<Rigidbody>().isKinematic = true;
                    StartCoroutine(SpiderJumpscare());
                }
            }
        }
        StartCoroutine(Countdown());
    }

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject && !bangOnVentOnCooldown) {
            bangOnVentOnCooldown = true;
            StartCoroutine(startCooldown());
            AudioSource.PlayClipAtPoint(bangOnVent, transform.position);
            if (nothingInVentTimer == 0) {
                nothingInVentTimer = nothingInVentTimerDefault;
                spiderApproachingVentTimer = spiderApproachingVentTimerDefault;
            }
        }
    }

    IEnumerator startCooldown() {
        yield return new WaitForSeconds(2);
        bangOnVentOnCooldown = false;
    }

    IEnumerator SpiderJumpscare() {
        int randomWait = Random.Range(jumpscareWaitMin, jumpscareWaitMax);
        yield return new WaitForSeconds(randomWait);
        canFall = true;
    }
}