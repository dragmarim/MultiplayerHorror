using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public AudioClip lightOnSound;
    public AudioClip lightOffSound;
    AudioSource audio;

    public GameObject roadKill;
    public GameObject button;
    public GameObject spotLight;
    public GameObject player;
    bool isSwitched;
    bool flipBackDown;
    
    void Start() {
        audio = GetComponent<AudioSource>();
        isSwitched = false;
    }

    void Update() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt != this.gameObject && isSwitched) {
            TurnOff();
        }
    }

    void TurnOff() {
        Debug.Log("turned off");
        audio.clip = lightOffSound;
        audio.Play();
        button.transform.localPosition = new Vector3(0, 0, 0);
        button.transform.localRotation = Quaternion.Euler(0, 0, 0);
        spotLight.SetActive(false);
        roadKill.GetComponent<CrawlTowardHouse>().lightOn = false;
        isSwitched = false;
    }

    void OnMouseOver() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
            if (Input.GetMouseButton(0) && !isSwitched) {
                Debug.Log("turned on");
                audio.clip = lightOnSound;
                audio.Play();
                button.transform.localPosition = new Vector3(0, 0, -0.02f);
                button.transform.localRotation = Quaternion.Euler(-45, 0, 0);
                spotLight.SetActive(true);
                roadKill.GetComponent<CrawlTowardHouse>().lightOn = true;
                isSwitched = true;
            } else if (!Input.GetMouseButton(0) && isSwitched) {
                TurnOff();
            }
        }
    }

    void OnMouseExit() {
        if (isSwitched) {
            TurnOff();
        }
    }
}
