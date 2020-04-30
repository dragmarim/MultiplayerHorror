using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public AudioClip windKeySound;
    public AudioClip windCarSound;

    public GameObject player;
    public GameObject Key;
    public GameObject mannequinManager;
    public float maxWoundUpTime;
    public float woundUpTime;
    public bool currentlyWinding = false;
    public bool moveTowardsPlayer = false;
    public bool moveAwayFromPlayer = false;
    public bool turnKey = false;
    float counter = 0;
    public float newRot = 0;
    public bool stoppedSound = true;

    void FixedUpdate()
    {
        if (woundUpTime >= 0) {
            woundUpTime -= 0.1f;
            transform.Rotate(0, -1.8f, 0);
            transform.position += transform.forward * Time.deltaTime * 0.8f;
            Key.transform.Rotate(0, 0, -1);
        } else if (!stoppedSound) {
            stoppedSound = true;
            GetComponent<AudioSource>().Stop();
            StartCoroutine(ShortDelay());
        }
    }

    void Update() {
        counter += Time.deltaTime;
        if (moveTowardsPlayer) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(3.8f, 1.494f, -5.3f), counter / 1.5f);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 156, 0), counter / 1.5f);
        }
        if (moveAwayFromPlayer) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(3.7f, 1.494f, -5.85f), counter / 1.5f);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), counter / 1.5f);
        }
        if (turnKey) {
            Debug.Log("rotation = " + Key.transform.rotation.y + " : " + Key.transform.localRotation.y);
			Key.transform.rotation = Quaternion.Slerp(Key.transform.rotation, Quaternion.Euler(0, Key.transform.eulerAngles.y, newRot), counter / 1.5f);
        }
    }

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
            this.transform.tag = "Untagged";
            player.GetComponent<BasicPlayerMovement>().WindUpCar();
            currentlyWinding = true;
        }
    }

    public void PowerUpCar() {
        StartCoroutine(PoweringUpCar());
    }

    IEnumerator PoweringUpCar() {
        counter = 0;
        moveTowardsPlayer = true;
        yield return new WaitForSeconds(0.5f);
        moveTowardsPlayer = false;
        GetComponent<AudioSource>().clip = windKeySound;
        GetComponent<AudioSource>().Play();
        counter = 0;
        newRot = Key.transform.eulerAngles.z+80;
        turnKey = true;
        yield return new WaitForSeconds(0.6f);
        turnKey = false;
        yield return new WaitForSeconds(0.3f);
        counter = 0;
        newRot = Key.transform.eulerAngles.z+80;
        turnKey = true;
        yield return new WaitForSeconds(0.6f);
        turnKey = false;
        yield return new WaitForSeconds(0.3f);
        counter = 0;
        newRot = Key.transform.eulerAngles.z+80;
        turnKey = true;
        yield return new WaitForSeconds(0.6f);
        turnKey = false;
        yield return new WaitForSeconds(0.2f);
        counter = 0;
        newRot = Key.transform.eulerAngles.z+80;
        turnKey = true;
        yield return new WaitForSeconds(0.6f);
        turnKey = false;
        yield return new WaitForSeconds(0.4f);
        counter = 0;
        newRot = Key.transform.eulerAngles.z+80;
        turnKey = true;
        yield return new WaitForSeconds(1);
        turnKey = false;
        yield return new WaitForSeconds(0.3f);
        counter = 0;
        moveAwayFromPlayer = true;
        yield return new WaitForSeconds(0.5f);
        moveAwayFromPlayer = false;
        player.GetComponent<BasicPlayerMovement>().DoneWinding();
        woundUpTime = maxWoundUpTime;
        GetComponent<AudioSource>().clip = windCarSound;
        GetComponent<AudioSource>().Play();
        stoppedSound = false;
    }

    IEnumerator ShortDelay() {
        yield return new WaitForSeconds(1);
        Debug.Log("delaySound");
        mannequinManager.GetComponent<MannequinManager>().CarFinished();
    }
}
