using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCooldown : MonoBehaviour
{
    public GameObject player;

    public int nothingInVentTimerDefault;
    int nothingInVentTimer;
    public int spiderApproachingVentTimerDefault;
    int spiderApproachingVentTimer;
    public int timeToUnscrew;
    int timeLeftUntilUnscrew;
    public int remainingScrews;

    public AudioClip spiderWalking;
    public AudioClip screwFalling;
    public AudioClip ventCoverFallOff;
    AudioSource aud;

    void Start() {
        aud = GetComponent<AudioSource>();
        nothingInVentTimer = nothingInVentTimerDefault;
        spiderApproachingVentTimer = spiderApproachingVentTimerDefault;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {
        yield return new WaitForSeconds(1);
        if (nothingInVentTimer > 0) {
            nothingInVentTimer -= 1;
            if (nothingInVentTimer == 0) {
                //aud.clip = spiderWalking;
                AudioSource.PlayClipAtPoint(spiderWalking, transform.position);
            }
        } else if (spiderApproachingVentTimer > 0) {
            spiderApproachingVentTimer -= 1;
            if (spiderApproachingVentTimer == 0) {
                timeLeftUntilUnscrew = timeToUnscrew;
            }
        } else {
            timeLeftUntilUnscrew -= 1;
            if (timeLeftUntilUnscrew == 0) {
                //aud.clip = screwFalling;
                AudioSource.PlayClipAtPoint(screwFalling, transform.position);
                remainingScrews -= 1;
                if (remainingScrews > 0) {
                    timeLeftUntilUnscrew = timeToUnscrew;
                } else {
                    this.tag = ("Untagged");
                    yield return new WaitForSeconds(2);
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;
                    yield return new WaitForSeconds(0.23f);
                    AudioSource.PlayClipAtPoint(ventCoverFallOff, transform.position);
                    yield return new WaitForSeconds(1);
                    GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        StartCoroutine(Countdown());
    }

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
            if (nothingInVentTimer == 0) {
                nothingInVentTimer = nothingInVentTimerDefault;
                spiderApproachingVentTimer = spiderApproachingVentTimerDefault;
            }
        }
    }
}