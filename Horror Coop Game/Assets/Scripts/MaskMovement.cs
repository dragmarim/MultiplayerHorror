using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskMovement : MonoBehaviour
{
    public GameObject maskSpawner;
    public GameObject player;
    bool floatUpFromSpawn;
    bool floatAcrossRoomForward;
    bool turnAround;
    bool floatAcrossRoomBackward;
    bool floatDownToSpawn;

    public void MaskStart() {
        floatUpFromSpawn = false;
        floatAcrossRoomForward = false;
        turnAround = false;
        floatAcrossRoomBackward = false;
        floatDownToSpawn = false;
        Debug.Log("Started");
        StartCoroutine(MoveMask());
    }

    void Update() {
        if (floatUpFromSpawn) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(4.65f, 2, 4.65f), 1 * Time.deltaTime);
		}
        if (floatAcrossRoomForward) {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(-3, 2, 1), 1 * Time.deltaTime);
		}
        if (turnAround) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 160, 0), 1 * Time.deltaTime);
		}
        if (floatAcrossRoomBackward) {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(4.65f, 2, 4.65f), 1 * Time.deltaTime);
		}
        if (floatDownToSpawn) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(4.65f, 0.5f, 4.65f), 1 * Time.deltaTime);
		}
    }

    IEnumerator MoveMask() {
        floatUpFromSpawn = true;
        yield return new WaitForSeconds(3);
        floatUpFromSpawn = false;
        floatAcrossRoomForward = true;
        yield return new WaitForSeconds(9);
        floatAcrossRoomForward = false;
        turnAround = true;
        yield return new WaitForSeconds(4);
        turnAround = false;
        floatAcrossRoomBackward = true;
        yield return new WaitForSeconds(9);
        floatAcrossRoomBackward = false;
        floatDownToSpawn = true;
        yield return new WaitForSeconds(1.5f);
        floatDownToSpawn = false;
        this.gameObject.SetActive(false);
        transform.position = new Vector3(4.65f, 0.5f, 4.65f);
        transform.rotation = Quaternion.Euler(0, -20, 0);
        maskSpawner.GetComponent<SpawnMaskOverTime>().SpawnMask();
    }
}