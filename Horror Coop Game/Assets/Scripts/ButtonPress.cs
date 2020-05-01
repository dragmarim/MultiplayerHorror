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
    public AudioClip buttonPress;
    public AudioClip buttonFailedToPress;

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject && gameManager.GetComponent<Randomize>().active && !completedButton) {
            AudioSource.PlayClipAtPoint(buttonPress, transform.position);
            gameManager.GetComponent<Randomize>().ButtonPress(buttonId, this.gameObject);
        } else if (player.GetComponent<BasicPlayerMovement>().lookingAt) {
            AudioSource.PlayClipAtPoint(buttonFailedToPress, transform.position);
        }
    }

    public void SuccessfulPress() {
        completedButton = true;
        runeMat.SetActive(false);
    }
}
