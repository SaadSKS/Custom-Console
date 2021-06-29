using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MobileAdjustments : MonoBehaviour
{
    public TextMeshProUGUI testText, textSizeT;
    public Slider slider;


    public void UpdateSlider()
    {
        testText.fontSize = slider.value;
        textSizeT.text = slider.value.ToString();
    }

}
