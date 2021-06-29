using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using env = System.Environment;

public class SysInfo : MonoBehaviour
{
    string info;

    public string GetSysInfo()
    {
        info = "<u>System Information:</u>" + env.NewLine
                + "Device Name:<pos=300>" + SystemInfo.deviceName + env.NewLine
                + "Device Type:<pos=300>" + SystemInfo.deviceType + env.NewLine
                + "Device Model:<pos=300>" + SystemInfo.deviceModel + env.NewLine
                + "Operating System:<pos=300>" + SystemInfo.operatingSystem + env.NewLine
                + "Operating Family:<pos=300>" + SystemInfo.operatingSystemFamily + env.NewLine
                + "Processor Type:<pos=300>" + SystemInfo.processorType + env.NewLine
                + "Processor Count:<pos=300>" + SystemInfo.processorCount+ env.NewLine
                + "Processor Frequency(MHz):<pos=300>" + SystemInfo.processorFrequency + env.NewLine
                + "Accelerometer Support:<pos=300>" + SystemInfo.supportsAccelerometer + env.NewLine
                + "Gyroscope Support:<pos=300>" + SystemInfo.supportsGyroscope + env.NewLine
                + "Vibration Support:<pos=300>" + SystemInfo.supportsVibration + env.NewLine
                + "Location Support:<pos=300>" + SystemInfo.supportsLocationService + env.NewLine
                + "Max Texture Size:<pos=300>" + SystemInfo.maxTextureSize + env.NewLine
                + "NPOT Support:<pos=300>" + SystemInfo.npotSupport + env.NewLine
                + "Battery Status:<pos=300>" + SystemInfo.batteryStatus + env.NewLine
                + "Battery Level:<pos=300>" + SystemInfo.batteryLevel;
        
        return info;
    }
}
