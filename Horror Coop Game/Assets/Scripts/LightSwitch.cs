using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject roadKill;
    public GameObject button;
    public GameObject spotLight;
    public GameObject player;
    bool isSwitched;
    bool flipBackDown;
    
    void Start() {
        isSwitched = false;
    }

    void Update() {
        if (Vector3.Distance(player.transform.position, transform.position) > 3.6f && isSwitched) {
            TurnOff();
        }
    }

    void TurnOff() {
        button.transform.localPosition = new Vector3(0, 0, 0);
        button.transform.localRotation = Quaternion.Euler(0, 0, 0);
        spotLight.SetActive(false);
        roadKill.GetComponent<CrawlTowardHouse>().lightOn = false;
        isSwitched = false;
    }

    void OnMouseOver() {
        if (Vector3.Distance(player.transform.position, transform.position) < 3.6f) {
            Debug.Log("Here");
            if (Input.GetMouseButton(0)) {
                Debug.Log("Worked");
                button.transform.localPosition = new Vector3(0, 0, -0.02f);
                button.transform.localRotation = Quaternion.Euler(-45, 0, 0);
                spotLight.SetActive(true);
                roadKill.GetComponent<CrawlTowardHouse>().lightOn = true;
                isSwitched = true;
            } else {
                TurnOff();
            }
        }
    }

    void OnMouseExit() {
        TurnOff();
    }
}
