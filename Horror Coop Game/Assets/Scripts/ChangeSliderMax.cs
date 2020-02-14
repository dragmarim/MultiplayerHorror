using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSliderMax : MonoBehaviour
{
    Slider localPlayerSlider;

    void Start() {
        localPlayerSlider = GetComponent<Slider>();
    }

    public void setSliderMax(float max) {
        localPlayerSlider.maxValue = max;
    }
}
