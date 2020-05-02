using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVictory : MonoBehaviour
{
    public GameObject player;

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
            Debug.Log("Victory");
            Time.timeScale = 0;
        }
    }
}
