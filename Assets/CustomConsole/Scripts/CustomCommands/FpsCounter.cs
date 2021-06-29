using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public int frequency = 30;
    int loop =0;
    //int fps;
    private void Update()
    {
        //fps = (Mathf.FloorToInt(1 / Time.unscaledDeltaTime)) - (Mathf.FloorToInt(1 / Time.unscaledDeltaTime)) % 5;
        //fpsText.text = "FPS:" + fps;
        //fpsText.text = "FPS:" + Mathf.FloorToInt(1 / Time.unscaledDeltaTime);

        if (loop == frequency)
        {
            loop = 0;
            fpsText.text = "FPS:" + Mathf.FloorToInt(1 / Time.unscaledDeltaTime);
        }
        loop++;
    }
}
