using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitOnCouch : MonoBehaviour
{
    public GameObject player;

    void OnMouseDown() {
        if (player.GetComponent<BasicPlayerMovement>().lookingAt == this.gameObject) {
            player.GetComponent<BasicPlayerMovement>().SitDown();
        }
    }
}
