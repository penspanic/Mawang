using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
public class JsonManager : MonoBehaviour
{
    TextAsset objectDataJson;
    TextAsset stageDataJson;
    TextAsset stageDesignDataJson;
    TextAsset princessScriptJson;
    TextAsset chapterDataJson;

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


    JsonData objectData;
    JsonData stageData;
    JsonData stageDesignData;
    JsonData princessScript;
    JsonData chapterData;


    public void CheckInstance()
    {

    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        objectDataJson      = Resources.Load<TextAsset>("TextFile/Object Data");
        stageDataJson       = Resources.Load<TextAsset>("TextFile/Stage Data");
        stageDesignDataJson = Resources.Load<TextAsset>("TextFile/Stage Design");
        princessScriptJson  = Resources.Load<TextAsset>("TextFile/Princess Script");
        chapterDataJson     = Resources.Load<TextAsset>("TextFile/Chapter Data");


        
        stageData           =   JsonMapper.ToObject(stageDataJson.text);
        stageDesignData     =   JsonMapper.ToObject(stageDesignDataJson.text);
        objectData          =   JsonMapper.ToObject(objectDataJson.text);
        princessScript      =   JsonMapper.ToObject(princessScriptJson.text);
        chapterData         =   JsonMapper.ToObject(chapterDataJson.text);

    }

    public string GetDescription(string name) // 유닛 설명 리턴
    {
        return objectData[name]["Description"].ToString();
    }

    public string GetJoke(string name) // 유닛에 관한 농담 리턴
    {
        return objectData[name]["Joke"].ToString();
    }

    public int GetCost(string name) // 아이템 가격 리턴
    {
        return (int)objectData[name]["Cost"];
    }

    public string GetType(string name)
    {
        return objectData[name]["Type"].ToString();
    }
    public string[] GetAppearEnemyName(string stageName) // 해당 스테이지에서 등장하는 적 이름 리턴
    {
        JsonData currStageData = stageData[stageName];

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
    
    public ChapterData GetChapterData(string chapterName)
    {
        chapterData = JsonMapper.ToObject(chapterDataJson.text)[chapterName];
        ChapterData returnData = new ChapterData();

        returnData.chapterName = chapterData["Chapter Name"].ToString();
        returnData.chapterDescription = chapterData["Chapter Description"].ToString();
        returnData.skillName = chapterData["Skill Name"].ToString();
        returnData.skillDescription = chapterData["Skill Description"].ToString();

        return returnData;
    }
}