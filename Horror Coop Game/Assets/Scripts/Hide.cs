using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public GameObject player;

    void OnMouseDown() {
        player.GetComponent<BasicPlayerMovement>().Hide();
        //this.gameObject.SetActive(false);
    }
}
