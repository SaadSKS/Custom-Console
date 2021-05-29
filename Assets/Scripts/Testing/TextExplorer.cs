using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextExplorer : MonoBehaviour
{
    public TextMeshProUGUI sampleText;
    public Image sampleBox;

    private void Start()
    {

    }

    public void GetTextInfo()
    {
        Debug.Log("Line Count: " + sampleText.textInfo.lineCount);
        Debug.Log("Line Info: " + sampleText.textInfo.lineInfo[0].lineHeight);
        Debug.Log("Line Info: " + sampleText.textInfo.lineInfo[1].lineHeight);
        Debug.Log("Line Info: " + sampleText.textInfo.lineInfo[2].lineHeight);

        sampleBox.rectTransform.sizeDelta = new Vector2(sampleBox.rectTransform.sizeDelta.x, (sampleText.textInfo.lineInfo[0].lineHeight)*(sampleText.textInfo.lineCount));

    }
}
