using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipUI : MonoBehaviour
{
    public TextAsset tipDataFile;
    public Text tipText;

    private string[] tipData;
    private int currIndex;

    private void Awake()
    {
        JsonData dataObject = JsonMapper.ToObject(tipDataFile.text);
        int count = dataObject["Tip Data"].Count;

        List<string> tipList = new List<string>();
        for (int i = 0; i < count; ++i)
            tipList.Add(dataObject["Tip Data"][i].ToString());
        tipData = tipList.ToArray();

        currIndex = Random.Range(0, tipData.Length);
        SetTip(currIndex);
    }

    private void SetTip(int index)
    {
        tipText.text = tipData[index];

        // and
    }

    public void OnTouched()
    {
        int newIndex;
        while (true)
        {
            newIndex = Random.Range(0, tipData.Length);
            if (newIndex != currIndex)
                break;
        }
        currIndex = newIndex;
        SetTip(currIndex);
    }
}