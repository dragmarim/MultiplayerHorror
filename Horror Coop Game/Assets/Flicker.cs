using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public float waitTime;
    public float flickerSpeed;
    public float maxLight;
    float currentTime;
    public bool flickerOn;
    public bool flickerOff;
    Light lt;

    void Start() {
        lt = this.GetComponent<Light>();
        flickerOn = false;
        flickerOff = false;
        currentTime = Time.deltaTime;
    }

    void Update() {
        /*
        currentTime += Time.deltaTime;
        if (currentTime >= waitTime) {
            currentTime -= waitTime;
            StartCoroutine(Blink());
        }
        */
        
        currentTime += Time.deltaTime;
        if (currentTime >= waitTime) {
            currentTime -= waitTime;
            flickerOn = true;
        }
        if (flickerOn) {
            if (lt.intensity < maxLight) {
                lt.intensity += flickerSpeed;
            } else {
                flickerOn = false;
                flickerOff = true;
            }
        }
        if (flickerOff) {
            if (lt.intensity > 0) {
                lt.intensity -= flickerSpeed;
            } else {
                flickerOff = false;
            }
        }
    }

    IEnumerator Blink() {
        lt.intensity = maxLight;
        yield return new WaitForSeconds(0.03f);
        lt.intensity = 0;
    }
}
