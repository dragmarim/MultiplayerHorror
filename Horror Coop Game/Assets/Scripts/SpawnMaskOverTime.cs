using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaskOverTime : MonoBehaviour
{
    public float delay;
    ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule emissionModule;
    public GameObject mask;

    void Start() {
        myParticleSystem = GetComponent<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
        StartCoroutine(SpawnMaskDelay());
    }

    public void SpawnMask() {
        StartCoroutine(SpawnMaskDelay());
    }

    IEnumerator SpawnMaskDelay() {
        float rate = 1;
        emissionModule.rateOverTime = rate;
        for (int i = 0; i < 50; i++) {
            yield return new WaitForSeconds(delay);
            rate += 1;
            emissionModule.rateOverTime = rate;
        }
        emissionModule.rateOverTime = 0;
        mask.SetActive(true);
        mask.GetComponent<MaskMovement>().MaskStart();
    }
}
