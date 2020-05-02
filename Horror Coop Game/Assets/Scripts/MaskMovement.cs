﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskMovement : MonoBehaviour
{
    public AudioClip screamClip;
    public bool willAttack;
    bool isActive;
    public GameObject childObject;
    public GameObject maskSpawner;
    public GameObject player;
    public GameObject target;
    public GameObject blackout;
    bool floatUpFromSpawn;
    bool floatAcrossRoomForward;
    bool turnAround;
    bool floatAcrossRoomBackward;
    bool floatDownToSpawn;
    bool attackPlayer;

    public void MaskStart() {
        isActive = false;
        willAttack = false;
        floatUpFromSpawn = false;
        floatAcrossRoomForward = false;
        turnAround = false;
        floatAcrossRoomBackward = false;
        floatDownToSpawn = false;
        Debug.Log("Started");
        StartCoroutine(MoveMask());
    }

    void Update() {
        if (!player.GetComponent<BasicPlayerMovement>().isHiding && willAttack && !isActive && player.GetComponent<BasicPlayerMovement>().canBeJumpscared) {
            GetComponentInChildren<Float>().enabled = false;
            isActive = true;
            player.GetComponent<BasicPlayerMovement>().DieFromMask(this.gameObject);
            StartCoroutine(DeathAnimation());
        }
        if (floatUpFromSpawn && !isActive) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(5, 2, 5), 1 * Time.deltaTime);
		}
        if (floatAcrossRoomForward && !isActive) {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(-3, 2, 1), 2 * Time.deltaTime);
		}
        if (turnAround && !isActive) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 160, 0), 1 * Time.deltaTime);
		}
        if (floatAcrossRoomBackward && !isActive) {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(5, 2, 5), 2 * Time.deltaTime);
		}
        if (floatDownToSpawn && !isActive) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(5, 0.5f, 5), 1 * Time.deltaTime);
		}
        if (attackPlayer) {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, 1.7f, target.transform.position.z), 30 * Time.deltaTime);
		}
    }

    IEnumerator MoveMask() {
        floatUpFromSpawn = true;
        yield return new WaitForSeconds(3);
        floatUpFromSpawn = false;
        willAttack = true;
        floatAcrossRoomForward = true;
        yield return new WaitForSeconds(4.5f);
        floatAcrossRoomForward = false;
        turnAround = true;
        yield return new WaitForSeconds(4);
        turnAround = false;
        floatAcrossRoomBackward = true;
        yield return new WaitForSeconds(4.5f);
        floatAcrossRoomBackward = false;
        floatDownToSpawn = true;
        yield return new WaitForSeconds(1.5f);
        floatDownToSpawn = false;
        willAttack = false;
        this.gameObject.SetActive(false);
        transform.position = new Vector3(4.65f, 0.5f, 4.65f);
        transform.rotation = Quaternion.Euler(0, -20, 0);
        maskSpawner.GetComponent<SpawnMaskOverTime>().SpawnMask();
    }

    IEnumerator DeathAnimation() {
        player.GetComponent<AudioSource>().clip = screamClip;
        player.GetComponent<AudioSource>().Play();
        childObject.GetComponent<Float>().enabled = false;
        childObject.transform.localPosition = new Vector3(0,0,0);
        transform.LookAt(new Vector3(player.transform.position.x, 1.7f, player.transform.position.z));
        transform.position = new Vector3(transform.position.x, 1.85f, transform.position.z);
        attackPlayer = true;
        Debug.Log(Vector3.Distance(transform.position, player.transform.position)/33);
        GetComponent<AudioSource>().enabled = false;
        yield return new WaitForSeconds(Vector3.Distance(transform.position, player.transform.position)/33);
        attackPlayer = false;
        yield return new WaitForSeconds(0.05f);
        blackout.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<AudioSource>().enabled = false;
    }
}