using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public GameObject runeMat;
    public int buttonId;
    public bool completedButton = false;

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject && gameManager.GetComponent<Randomize>().active && !completedButton) {
            gameManager.GetComponent<Randomize>().ButtonPress(buttonId, this.gameObject);
        } else if (player.GetComponent<BasicPlayerMovement>().lookingAt) {
            Debug.Log("Button not active");
        }
    }

    public void SuccessfulPress() {
        completedButton = true;
        runeMat.SetActive(false);
    }
}
