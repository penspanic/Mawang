using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
public class JsonManager : MonoBehaviour
{
    TextAsset objectDataJson;
    TextAsset stageDataJson;
    TextAsset princessScriptJson;

    #region Singleton
    static JsonManager _instance;
    public static JsonManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newObj = new GameObject("Json Manager");
                _instance = newObj.AddComponent<JsonManager>();
            }
            return _instance;
        }
    }
    #endregion

    public void CheckInstance()
    {

    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        objectDataJson = Resources.Load<TextAsset>("TextFile/Object Data");
        stageDataJson = Resources.Load<TextAsset>("TextFile/Stage Data");
        princessScriptJson = Resources.Load<TextAsset>("TextFile/Princess Script");
    }

    public string GetDescription(string name) // 유닛 설명 리턴
    {
        JsonData objectData = JsonMapper.ToObject(objectDataJson.text);
        return objectData[name]["Description"].ToString();
    }

    public string GetJoke(string name) // 유닛에 관한 농담 리턴
    {
        JsonData objectData = JsonMapper.ToObject(objectDataJson.text);
        return objectData[name]["Joke"].ToString();
    }

    public int GetCost(string name) // 아이템 가격 리턴
    {
        JsonData objectData = JsonMapper.ToObject(objectDataJson.text);
        return (int)objectData[name]["Cost"];
    }
    public string[] GetAppearEnemyName(string stageName) // 해당 스테이지에서 등장하는 적 이름 리턴
    {
        Debug.Log(stageName);
        JsonData currStageData = JsonMapper.ToObject(stageDataJson.text)[stageName];

        List<string> enemyNameList = new List<string>();
        for (int i = 0; i < currStageData.Count; i++)
        {
            enemyNameList.Add(currStageData[i].ToString());
        }
        return enemyNameList.ToArray();
    }

    public string[] GetPrincessScript(string chapterName)
    {
        JsonData princessesData = JsonMapper.ToObject(princessScriptJson.text)["Princesses"];
        List<string> scriptList = new List<string>();

        for (int i = 0; i < princessesData.Count;i++)
        {
            if(princessesData[i]["Chapter"].ToString() == chapterName)
            {
                for(int j=0;j<princessesData[i]["Scripts"].Count;j++)
                {
                    scriptList.Add(princessesData[i]["Scripts"][j].ToString());
                }
                break;
            }
        }
        return scriptList.ToArray();
    }
}