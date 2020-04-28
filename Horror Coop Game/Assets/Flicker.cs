using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public float waitTime;
    public float flickerSpeed;
    public float maxLight;
    float currentTime;
    public bool lightIsOff;
    bool flickerOn;
    bool flickerOff;
    Light lt;

    void Start() {
        lightIsOff = true;
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
            lightIsOff = false;
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
                lightIsOff = true;
            }
        }
    }

    IEnumerator Blink() {
        lt.intensity = maxLight;
        yield return new WaitForSeconds(0.03f);
        lt.intensity = 0;
    }
}
