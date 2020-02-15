using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicked : MonoBehaviour
{
    public int runeID;
    public GameObject randomizeRuneOrder;
    
    void OnMouseDown() {
        randomizeRuneOrder.GetComponent<Randomize>().NextNumber(runeID);
    }
}
