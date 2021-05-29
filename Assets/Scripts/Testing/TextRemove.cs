using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextRemove : MonoBehaviour
{
    public TextMeshProUGUI test;
    public bool remove = false;
    int indexNL = 0;
    int lines;
    int counter = 0;
    

    void Start()
    {

    }

    private void Update()
    {
        lines = test.textInfo.lineCount;
        //Debug.Log(" Line Count: " + lines + " Update Count: " + counter);
        counter++;
        if (lines != 0 && remove)
        {
            /*for(int i = 0; i < 2; i++)
            {
                indexNL = test.text.IndexOf(System.Environment.NewLine, indexNL);
            }*/
            //indexNL = test.text.IndexOf('B', 1);
            indexNL = test.textInfo.lineInfo[0].characterCount;
            //Debug.Log(lines+" index: "+indexNL);
            Debug.Log(test.textInfo.lineInfo[0].characterCount);
        }
    }

    public void ShortenText()
    {
        test.text = test.text.Substring(indexNL);
    }

}
