using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script ensure the correct console is used based on the platform the game is running on. Also used to activate and deactivate the console
//Currently the same console prefab is used for both Desktop and mobile devices.

public class ConsoleManager : MonoBehaviour
{
    public GameObject consolePanel;
    
    bool isMobile=false;
    bool isActivated = false;

    ActionMap am;

    private void Awake()
    {
        am = new ActionMap();

        am.console.Toggle.performed += ctx => ToggleConsole();

    }

    private void Start()
    {
        DetectPlatform();
    }

    private void DetectPlatform()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isMobile = true;
        }
    }

    public void ActivateConsole()
    {
        if (!isMobile)
        {
            consolePanel.SetActive(true);
        }
        else
        {
            //activate the mobile console gameobject
        }
    }

    public void DeactivateConsole()
    {
        if (!isMobile)
        {
            consolePanel.SetActive(false);
        }
        else
        {
            //deactivate the mobile console gameobject
        }
    }

    public void ToggleConsole()
    {
        if (isActivated)
        {
            if (!isMobile)
            {
                consolePanel.SetActive(false);
            }
            else
            {
                //deactivate the mobile console gameobject
            }
            isActivated = false;
        }
        else
        {
            if (!isMobile)
            {
                consolePanel.SetActive(true);
            }
            else
            {
                //activate the mobile console gameobject
            }
            isActivated = true;
        }
    }

    private void OnEnable()
    {
        am.Enable();
    }

    private void OnDisable()
    {
        am.Disable();
    }

}
