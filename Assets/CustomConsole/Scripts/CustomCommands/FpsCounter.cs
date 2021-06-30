using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public int frequency = 30;
    int loop =0;

    private void Update()
    {
        if (loop == frequency)
        {
            loop = 0;
            fpsText.text = "FPS:" + Mathf.FloorToInt(1 / Time.unscaledDeltaTime);
        }
        loop++;
    }
}
