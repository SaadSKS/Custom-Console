using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using env = System.Environment;
using UnityEngine.Events;

public class ConsoleBehaviour : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public GameObject ContentBox;
    public TMP_InputField inputFieldTMP;

    enum ConsoleState { Inactive, Ready, Preprocessed, CommandStarted, CommandEnded, OverwriteCheck, CheckHeight};
    ConsoleState consoleState = ConsoleState.Inactive;
    
    string consoleLog= "--------------------- CONSOLE INITIATED ---------------------" + env.NewLine;
    string currentCommand = "";
    int lineCount = 1;
    int prevLineCount = 0;
    int maxLines = 30;
    int visibleLines = 9;
    int spacing = 23;
    int fontSize = 20;
    int baseHeight = 220;
    RectTransform rt;
    Vector2 originalSizeRT;
    bool logRunning = false;
    IEnumerator DT; //referencing DemoTest Coroutine
    string newInfo; //latest info appended to log text
    UnityEvent inputDeselect;

    //List of Commands
    public List<string> commandList = new List<string>() { "CLR", "TEST" };

    //previous user entries
    List<string> prevCommands = new List<string>();
    
    
    private void Start()
    {
        DT = DemoText();
        rt = ContentBox.GetComponent<RectTransform>();
        originalSizeRT = rt.sizeDelta;
        prevLineCount = logText.textInfo.lineCount;
        inputFieldTMP.onEndEdit.RemoveAllListeners();
        inputFieldTMP.onEndEdit.AddListener(AutoSubmit);
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

    public void StartDemoText()
    {
        if (!logRunning)
        {
            StartCoroutine(DT);
        }
    }

    public void StopDemoText()
    {
        if (logRunning)
        {
            StopCoroutine(DT);
            DT = DemoText();
            logRunning = false;
        }
    }


    //insert line of text/paragraph periodically. Appends new information to current textbox. Overwrites when exceeds max size/line count.
    IEnumerator DemoText()
    {
        logRunning = true;

        while (prevLineCount < maxLines)
        {
            //consoleLog = consoleLog + sampleParagraph + " "+ lineCount.ToString() + env.NewLine;
            logText.text = consoleLog;
            yield return null;
            //yield return new WaitForSeconds(1);
        }
        logRunning = false;
    }

    public void AutoSubmit(string str)
    {
        //press enter to process command instead of using submit button
        if (consoleState == ConsoleState.Ready)
        {
            Preprocess();
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

    public void Preprocess()
    {
        if (consoleState == ConsoleState.Ready)
        {
            currentCommand = inputFieldTMP.text;
            currentCommand = currentCommand.ToUpper();
            consoleState = ConsoleState.Preprocessed;
            /*if (commandList.Contains(currentCommand))
            {
                //Found
                Invoke(currentCommand, 0);
            }
            else
            {
                //Invalid. Type help for command list.
                Invoke("INVALIDCOMMAND", 0);
            }*/
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
        rt.sizeDelta = new Vector2(originalSizeRT.x, newHeight);
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
        logText.text = "--------------------- CONSOLE INITIATED ---------------------" + env.NewLine;
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
