using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
public class TipUI : MonoBehaviour
{
    public TextAsset tipDataFile;
    public Text tipText;

    string[] tipData;
    int currIndex;
    void Awake()
    {
        JsonData dataObject = JsonMapper.ToObject(tipDataFile.text);
        int count = dataObject["Tip Data"].Count;

        List<string> tipList = new List<string>();
        for (int i = 0; i < count; i++)
            tipList.Add(dataObject["Tip Data"][i].ToString());
        tipData = tipList.ToArray();

        currIndex = Random.Range(0, tipData.Length);
        SetTip(currIndex);
    }

    void SetTip(int index)
    {
        tipText.text = tipData[index];
        
        // and
    }

    public void OnTouched()
    {
        int newIndex;
        while(true)
        {
            newIndex = Random.Range(0, tipData.Length);
            if (newIndex != currIndex)
                break;
        }
        currIndex = newIndex;
        SetTip(currIndex);
    }
}
