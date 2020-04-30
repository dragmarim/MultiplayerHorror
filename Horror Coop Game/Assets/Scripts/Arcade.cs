using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade : MonoBehaviour
{
    public GameObject mannequinManager;
    public GameObject player;
    public bool arcadeOn = false;
    public float arcadeOnTimer;
    public float requiredTimeForArcadeOn;
    public bool hasDemanded = false;

    public GameObject blackScreen;
    public GameObject afterScape;
    public GameObject apeKing;
    public GameObject climb;
    public GameObject rubber;

    void Update() {
        if (arcadeOn) {
            arcadeOnTimer += Time.deltaTime;
            if (arcadeOnTimer >= requiredTimeForArcadeOn && !hasDemanded) {
                hasDemanded = true;
                mannequinManager.GetComponent<MannequinManager>().DemandTurnArcadeOff();
            }
        }
    }

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
            if (!arcadeOn) {
                mannequinManager.GetComponent<MannequinManager>().TurnedOnArcade();
                hasDemanded = false;
                arcadeOnTimer = 0;
                arcadeOn = true;
                this.name = "Turn Off Arcade";
                Random.seed = (int)System.DateTime.Now.Ticks;
                if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
                    int rand = Random.Range(1, 5);
                    blackScreen.SetActive(false);
                    if (rand == 1) {
                        afterScape.SetActive(true);
                    } else if (rand == 2) {
                        apeKing.SetActive(true);
                    } else if (rand == 3) {
                        climb.SetActive(true);
                    } else {
                        rubber.SetActive(true);
                    }
                }
            } else {
                if (hasDemanded) {
                    mannequinManager.GetComponent<MannequinManager>().DoneWithArcade();
                } else {
                    mannequinManager.GetComponent<MannequinManager>().FailedArcade();
                }
                arcadeOn = false;
                this.name = "Turn On Arcade";
                blackScreen.SetActive(true);
                afterScape.SetActive(false);
                apeKing.SetActive(false);
                climb.SetActive(false);
                rubber.SetActive(false);
            }
        }
    }
}
