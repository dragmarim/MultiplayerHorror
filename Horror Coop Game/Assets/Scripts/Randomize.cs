using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize : MonoBehaviour
{
    List<int> runesAvailable;
    List<int> runeOrder;
    void Start()
    {
        Random.InitState(1);
        runeOrder = new List<int>();
        runesAvailable = new List<int>();

        for (int i = 0; i < 8; i++) {
            runesAvailable.Add(i);
        }

        for (int i = 0; i < 8; i++) {
            int random = Random.Range(0, runesAvailable.Count-1);
            runeOrder.Add(runesAvailable[random]);
            runesAvailable.RemoveAt(random);
            Debug.Log(runeOrder[i]);
        }
    }
}
