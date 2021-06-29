using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounterEnabler : MonoBehaviour
{
    public GameObject fpsPanel;
    bool fpsActive = false;
    string msg = "";

    public string ToggleFPS()
    {
        if (fpsActive)
        {
            fpsPanel.SetActive(false);
            msg = "FPS Counter DISABLED";
        }
        else
        {
            fpsPanel.SetActive(true);
            msg = "FPS Counter ENABLED";
        }
        fpsActive = !fpsActive;
        return msg;
    }
}
