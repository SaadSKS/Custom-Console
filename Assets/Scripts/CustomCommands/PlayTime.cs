using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTime : MonoBehaviour
{
    float rawTime, ms, s, min, hr;

    public string CalcTime()
    {
        string sessionTime;

        rawTime = 20308.2442f;
        //rawTime = Time.fixedUnscaledTime;
        ms = Mathf.Round(rawTime % 1 * 100);
        s = rawTime - rawTime % 1;
        if (s >= 60) 
        {
            min = (s - s % 60) / 60;
            s = s % 60;
        }
        if (min >= 60)
        {
            hr = (min - min % 60) / 60;
            min = min % 60;
        }

        //sessionTime = rawTime + "HR:" + hr + " MIN:" + min + " S:" + s + " MS:" + ms;

        sessionTime ="Session play time: "+ NumberFormatter(hr.ToString()) + ":" + NumberFormatter(min.ToString()) + ":" + NumberFormatter(s.ToString());

        return sessionTime;
    }

    string NumberFormatter(string str)
    {
        if (str.Length < 2)
        {
            str = "0" + str;
        }
        return str;
    }
}
