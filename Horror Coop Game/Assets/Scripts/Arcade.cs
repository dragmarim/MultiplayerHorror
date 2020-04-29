using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade : MonoBehaviour
{
    public GameObject player;
    public bool arcadeOn = false;

    public GameObject blackScreen;
    public GameObject afterScape;
    public GameObject apeKing;
    public GameObject climb;
    public GameObject rubber;

    void OnMouseDown() {
        if (!arcadeOn) {
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
