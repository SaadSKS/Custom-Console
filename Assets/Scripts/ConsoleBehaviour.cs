using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using env = System.Environment;
using UnityEngine.Events;
using System.Linq;

public class ConsoleBehaviour : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public GameObject contentBox;
    public TMP_InputField inputFieldTMP;
    public RectTransform mainRT;
    public RectTransform scrollViewRT;
    public RectTransform stretchButtonRT;


    enum ConsoleState { Inactive, Ready, Preprocessed, CommandStarted, CommandEnded, OverwriteCheck, CheckHeight};
    ConsoleState consoleState = ConsoleState.Inactive;

    //string consoleLog = "--------------------- CONSOLE INITIATED ---------------------" + env.NewLine +" "+ env.NewLine;
    string currentCommand = "";
    //int lineCount = 1;
    int prevLineCount = 0;
    int maxLines = 30;
    //int visibleLines = 9;
    //int spacing = 23;
    //int fontSize = 20;
    public int baseHeight = 220;
    RectTransform contentBoxRT;
    Vector2 originalSizeRT;
    //bool logRunning = false;
    //IEnumerator DT; //referencing DemoTest Coroutine
    string newInfo; //latest info appended to log text
    //UnityEvent inputDeselect;

    //Resize MainRT
    bool isMaximized = false;

    //List of Commands
    List<string> commandList = new List<string>() { "CLR", "FPS", "HELP", "PLAYTIME", "SYS", "TEST" };

    //previous user entries
    List<string> prevCommands = new List<string>();


    //Custom Command Properties
    FpsCounterEnabler fpsCounterEnabler;
    PlayTime playTime;
    SysInfo sysInfo;





    private void Start()
    {
        //DT = DemoText();
        commandList.Sort();
        contentBoxRT = contentBox.GetComponent<RectTransform>();
        originalSizeRT = contentBoxRT.sizeDelta;

        prevLineCount = logText.textInfo.lineCount;
        inputFieldTMP.onEndEdit.RemoveAllListeners();
        inputFieldTMP.onEndEdit.AddListener(AutoSubmit);
        

        //Get custom command scripts
        fpsCounterEnabler = GetComponent<FpsCounterEnabler>();
        playTime = GetComponent<PlayTime>();
        sysInfo = GetComponent<SysInfo>();
    }

    private void Update()
    {
        if (consoleState == ConsoleState.Ready)
        {
            //Do nothing. 
        }
        else if (consoleState == ConsoleState.Preprocessed)
        {
            //skip one update cycle, to ensure commandprocess is not called by OnDisable
            ProcessCommand();

        }
        else if (consoleState == ConsoleState.CommandEnded)
        {
            //skip one update cycle, to update textInfo
            consoleState = ConsoleState.OverwriteCheck;
        }
        else if (consoleState == ConsoleState.OverwriteCheck)
        {
            //Overwrite check
            //OverwriteCheck(logText.textInfo.lineCount);

            //skip one update cycle, to update textInfo
            consoleState = ConsoleState.CheckHeight;
        }
        else if (consoleState == ConsoleState.CheckHeight) 
        {
            //check if line count changed. readjust if change found
            if (prevLineCount != logText.textInfo.lineCount)
            {
                prevLineCount = logText.textInfo.lineCount;

                ReadjustContentBox(prevLineCount);
            }
            inputFieldTMP.text = "";
            inputFieldTMP.ActivateInputField();
            consoleState = ConsoleState.Ready;
        }

        //Debug.Log(consoleState + " Line count: " + logText.textInfo.lineCount);
    }

    public void ResizeConsole()
    {
        if (isMaximized)
        {
            //Minimize
            mainRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 310);
            scrollViewRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 20, 220);
            stretchButtonRT.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            //Maximize
            mainRT.offsetMin = new Vector2(0, 0);
            mainRT.offsetMax = new Vector2(0, 0);
            mainRT.anchorMin = new Vector2(0, 0);
            mainRT.anchorMax = new Vector2(1, 1);
            mainRT.pivot = new Vector2(0.5f, 0.5f);
            
            scrollViewRT.anchorMin = new Vector2(0, 0);
            scrollViewRT.anchorMax = new Vector2(1, 1);
            scrollViewRT.offsetMin = new Vector2(20, 70);
            scrollViewRT.offsetMax = new Vector2(-20, -20);
            scrollViewRT.pivot = new Vector2(0.5f, 1);

            stretchButtonRT.rotation = Quaternion.Euler(0, 0, 180);
        }
        isMaximized = !isMaximized;
        //Recheck height state?

    }

    public void AutoSubmit(string str)
    {
        //press enter to process command instead of using submit button
        if (consoleState == ConsoleState.Ready)
        {
            Preprocess();
        }
     }

    public void Preprocess()
    {
        if (consoleState == ConsoleState.Ready)
        {
            /*
            currentCommand = inputFieldTMP.text;
            currentCommand = currentCommand.ToUpper();
            consoleState = ConsoleState.Preprocessed;
            */
            if (inputFieldTMP.text.Length>0)
            {
                currentCommand = inputFieldTMP.text;
                currentCommand = currentCommand.ToUpper();
                consoleState = ConsoleState.Preprocessed;
            }
            else
            {
                consoleState = ConsoleState.Ready;
            }
        }

    }

    public void ProcessCommand()
    {
        prevCommands.Add(currentCommand);
        if (commandList.Contains(currentCommand))
        {
            //Found
            Invoke(currentCommand, 0);
        }
        else
        {
            //Invalid. Type help for command list.
            Invoke("INVALIDCOMMAND", 0);
        }
    }
    
    
    private void OverwriteCheck(int newLines)
    {
        if (newLines > maxLines)
        {
            int extraLines = (newLines - maxLines);
            int newLineIndex = 0;
            for (int i = 0; i < extraLines; i++)
            {
                newLineIndex += logText.textInfo.lineInfo[i].characterCount;
                
                if (i != 0)
                {
                    newLineIndex--;
                }
            }
            logText.text = logText.text.Substring(newLineIndex);
            string str = logText.text;
            if (string.IsNullOrWhiteSpace(str.Substring(0, 1)))
            {
                Debug.Log("WHITESPACE");
                str.TrimStart();
                logText.text = str;
            }
        }
        /*if (newLines > maxLines)
        {
            int index = logText.text.IndexOf(System.Environment.NewLine);
            logText.text = logText.text.Substring(index + System.Environment.NewLine.Length);
            Debug.Log("OVERWRITED: " + index);
        }*/

    }

    private void ReadjustContentBox(int newLines)
    {
        //calc new height
        float newHeight = 0;
        newHeight = ((logText.textInfo.lineInfo[0].lineHeight) * (newLines + 1)); //height of one line multiplied with (lineCount +1)

        //Ensure height doesnt fall below base height
        if (newHeight < baseHeight)
        {
            newHeight = baseHeight; 
        }
        //set new height
        contentBoxRT.sizeDelta = new Vector2(originalSizeRT.x, newHeight);
    }

    private void StartInputField()
    {
        inputFieldTMP.ActivateInputField();
        inputFieldTMP.onEndEdit.RemoveAllListeners();
        inputFieldTMP.onEndEdit.AddListener(AutoSubmit);
        consoleState = ConsoleState.Ready;
    }
    
    private void OnEnable()
    {
        //Time Freeze might needed to postponed if tweens or animated appear effects are used on console gameobject
        Time.timeScale = 0;
        Invoke("StartInputField", 0); //Invoke used since direct activation doesnt work :(
    }

    private void OnDisable()
    {
        inputFieldTMP.onEndEdit.RemoveListener(AutoSubmit);
        consoleState = ConsoleState.Inactive;
        EventSystem.current.SetSelectedGameObject(null);
        inputFieldTMP.text = "";
        Time.timeScale = 1;
    }














    //_____________________________________________________________________________________________________________________//
    //                                                                                                                     //
    //                                               CUSTOM COMMANDS GO HERE                                               //
    //_____________________________________________________________________________________________________________________//


    public void INVALIDCOMMAND()
    {
        consoleState = ConsoleState.CommandStarted;
        newInfo = "Unknown command. Type (Help) to view a list of known commands" + env.NewLine;
        logText.text = logText.text + newInfo;
        consoleState = ConsoleState.CommandEnded;
    }

    public void CLR()
    {
        //Clear console screen
        consoleState = ConsoleState.CommandStarted;
        logText.text = "--------------------- CONSOLE INITIATED ---------------------" + env.NewLine + " " + env.NewLine;
        consoleState = ConsoleState.CommandEnded;
    }

    public void HELP()
    {
        //Displays a list of all known commands
        consoleState = ConsoleState.CommandStarted;
        newInfo = "<u>List of all known commands:</u> " + env.NewLine;
        for (int i = 0; i < commandList.Count; i++)
        {
            newInfo = newInfo + commandList[i] + env.NewLine;
        }
        logText.text = logText.text + newInfo;
        consoleState = ConsoleState.CommandEnded;

    }
    
    public void FPS()
    {
        //Toggle FPS Counter visibility
        consoleState = ConsoleState.CommandStarted;
        newInfo = fpsCounterEnabler.ToggleFPS() + env.NewLine;
        logText.text = logText.text + newInfo;
        consoleState = ConsoleState.CommandEnded;
    }

    public void PLAYTIME()
    {
        //Displays play time (only current session time is implemented)
        consoleState = ConsoleState.CommandStarted;
        newInfo = playTime.CalcTime() + env.NewLine;
        logText.text = logText.text + newInfo;
        consoleState = ConsoleState.CommandEnded;
    }

    public void SYS()
    {
        //Displays System Information
        consoleState = ConsoleState.CommandStarted;
        newInfo = sysInfo.GetSysInfo() + env.NewLine;
        logText.text = logText.text + newInfo;
        consoleState = ConsoleState.CommandEnded;
    }

    public void TEST()
    {
        //Display sample log text in console
        consoleState = ConsoleState.CommandStarted;
        newInfo = "Sample Game Info - 1 " + env.NewLine + "Sample Game Info - 2 " + env.NewLine + "Sample Game Info - 3 " + env.NewLine;
        logText.text = logText.text + newInfo;
        consoleState = ConsoleState.CommandEnded;
    }


    public void SAMPLECOMMAND()
    {
        consoleState = ConsoleState.CommandStarted;
        //Command behaviour goes here. Any information that is to be displayed in log should be placed in "newInfo". newInfo must contain newline at end of its string.

        consoleState = ConsoleState.CommandEnded;
    }



}
