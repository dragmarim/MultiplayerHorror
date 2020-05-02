using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlTowardHouse : MonoBehaviour
{
    public GameObject window;
    public bool lightOn;
    bool climbUpOverHill;
    bool startClimbingDownHill;
    bool climbingDownHill;
    bool runAway;
    public float crawlSpeed;
    public bool windowBreak;
    public bool windowIsBroken;
    public bool canPeer;
    public bool peerWait;
    public bool peeringThroughWindow = false;
    public GameObject peeringCrawlBoi;
    public GameObject player;
    public bool nextToHouse;
    public bool tooLate = false;
    public bool triggerJumpscare = false;
    public GameObject blackout;

    public float crawlStartDelayMin = 0;
    public float crawlStartDelayMax = 0;

    void Start() {
        nextToHouse = false;
        peerWait = false;
        peeringThroughWindow = false;
        windowBreak = false;
        windowIsBroken = false;
        climbUpOverHill = false;
        startClimbingDownHill = false;
        climbingDownHill = false;
        lightOn = false;
        runAway = false;
        StartCoroutine(CrawlStart());
    }

    void Update() {
        if (canPeer && player.transform.eulerAngles.y > 40 && player.transform.eulerAngles.y < 125 ) {
            nextToHouse = false;
            peeringCrawlBoi.SetActive(true);
            peeringThroughWindow = true;
            StartCoroutine(WaitToJumpscare()); 
        }
        if (climbUpOverHill) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-78, 7.5f, -10), 6 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-10, 75, 3), 2 * Time.deltaTime);
        }
        if (startClimbingDownHill) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-76, 7.25f, -9.75f), 6 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(5.5f, 85, 1.8f), 4 * Time.deltaTime);
        }
        if (climbingDownHill && Vector3.Distance(new Vector3(-9.5f, 0.5f, -5), transform.position) > 0.1f && !lightOn) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-9.5f, 0.2f, -5), crawlSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, new Vector3(-9.5f, 0.2f, -5)) < 1) {
                if (windowIsBroken && !peerWait) {
                    nextToHouse = true;
                    peerWait = true;
                    StartCoroutine(PeerThroughWindow());
                } else if (!windowBreak) {
                    nextToHouse = true;
                    windowBreak = true;
                    StartCoroutine(WaitToBreakWindow());
                }
            }
        }
        if (lightOn && peeringThroughWindow && !tooLate) {
            canPeer = false;
            peerWait = false;
            peeringThroughWindow = false;
        }
        if (lightOn && Vector3.Distance(new Vector3(-9.5f, 0.5f, -5), transform.position) < 45 && !nextToHouse && !tooLate && !runAway) {
            runAway = true;
            climbingDownHill = false;
            StartCoroutine(CrawlStart());
        } else if (lightOn && tooLate && !triggerJumpscare) {
            triggerJumpscare = true;
            Jumpscare();
        }
        if (runAway) {
            peeringCrawlBoi.transform.position = new Vector3(peeringCrawlBoi.transform.position.x, peeringCrawlBoi.transform.position.y-0.25f, peeringCrawlBoi.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, -100), 30 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(5.5f, 175, 1.8f), 4 * Time.deltaTime);
        }
    }

    void Jumpscare() {
        if (player.GetComponent<BasicPlayerMovement>().canBeJumpscared) {
            GetComponent<AudioSource>().Play();
            player.GetComponent<BasicPlayerMovement>().DieFromRoadKill();
            peeringCrawlBoi.transform.position = player.transform.position + player.transform.forward;
            peeringCrawlBoi.transform.LookAt(player.transform.position);
            peeringCrawlBoi.transform.eulerAngles = new Vector3(0, peeringCrawlBoi.transform.eulerAngles.y, peeringCrawlBoi.transform.eulerAngles.z);
            StartCoroutine(EndOfJumpscare());
        }
    }

    IEnumerator CrawlStart() {
        float rand = Random.Range(crawlStartDelayMin, crawlStartDelayMax);
        yield return new WaitForSeconds(rand);
        peeringCrawlBoi.SetActive(false);
        peeringCrawlBoi.transform.position = new Vector3(-8.72f, 0, 0.18f);
        runAway = false;
        transform.position = new Vector3(-81, 6, -11);
        transform.rotation = Quaternion.Euler(-31, 74, 3);
        climbUpOverHill = true;
        yield return new WaitForSeconds(0.5f);
        climbUpOverHill = false;
        startClimbingDownHill = true;
        yield return new WaitForSeconds(0.5f);
        startClimbingDownHill = false;
        climbingDownHill = true;
        StartCoroutine(BriefDelay());
    }

    IEnumerator BriefDelay() {
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(ChangeSpeed());
    }

    IEnumerator ChangeSpeed() {
        //crawlSpeed = 1.5f;
        crawlSpeed = 3;
        yield return new WaitForSeconds(0.6165f);
        //crawlSpeed = 5;
        crawlSpeed = 10;
        yield return new WaitForSeconds(0.2f);
        if (climbingDownHill) {
            StartCoroutine(ChangeSpeed());
        }
    }

    IEnumerator WaitToBreakWindow() {
        int randomNumber = Random.Range(4, 8);
        yield return new WaitForSeconds(randomNumber);
        runAway = true;
        climbingDownHill = false;
        StartCoroutine(CrawlStart());
        window.GetComponent<BreakableWindow>().collide();
        windowIsBroken = true;
        nextToHouse = false;
    }

    IEnumerator PeerThroughWindow() {
        int randomNumber = Random.Range(4, 8);
        yield return new WaitForSeconds(randomNumber);
        canPeer = true;
    }

    IEnumerator WaitToJumpscare() {
        yield return new WaitForSeconds(5);
        if (peeringThroughWindow) {
            tooLate = true;
        }
        yield return new WaitForSeconds(5);
        if (peeringThroughWindow && !triggerJumpscare) {
            triggerJumpscare = true;
            Jumpscare();
        }
    }

    IEnumerator EndOfJumpscare() {
        yield return new WaitForSeconds(1.2f);
        blackout.SetActive(true);
    }
}
