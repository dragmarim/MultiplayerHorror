using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlTowardHouse : MonoBehaviour
{
    public bool lightOn;
    bool climbUpOverHill;
    bool startClimbingDownHill;
    bool climbingDownHill;
    bool runAway;

    void Start() {
        climbUpOverHill = false;
        startClimbingDownHill = false;
        climbingDownHill = false;
        lightOn = false;
        runAway = false;
        StartCoroutine(CrawlStart());
    }

    void Update() {
        if (climbUpOverHill) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-78, 7.5f, -10), 3 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-10, 75, 3), 1 * Time.deltaTime);
        }
        if (startClimbingDownHill) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-76, 7.25f, -9.75f), 3 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(5.5f, 85, 1.8f), 2 * Time.deltaTime);
        }
        if (climbingDownHill && Vector3.Distance(new Vector3(-9.5f, 0.5f, -5), transform.position) > 0.1f && !lightOn) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-9.5f, 0.5f, -5), 4 * Time.deltaTime);
        }
        if (lightOn && Vector3.Distance(new Vector3(-9.5f, 0.5f, -5), transform.position) < 45) {
            runAway = true;
            climbingDownHill = false;
            CrawlStart();
        }
        if (runAway) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, -100), 30 * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(5.5f, 175, 1.8f), 4 * Time.deltaTime);
        }
    }

    IEnumerator CrawlStart() {
        yield return new WaitForSeconds(2);
        runAway = false;
        transform.position = new Vector3(-81, 6, -11);
        transform.rotation = Quaternion.Euler(-31, 74, 3);
        climbUpOverHill = true;
        yield return new WaitForSeconds(1);
        climbUpOverHill = false;
        startClimbingDownHill = true;
        yield return new WaitForSeconds(1);
        startClimbingDownHill = false;
        climbingDownHill = true;
    }
}
