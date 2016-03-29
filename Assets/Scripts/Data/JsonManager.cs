using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public struct StagePattern
{
    public StagePattern(string[] patternsName, float interval, float princessCoolTime, float buffDuration,
        float earlyTimeInterval, float earlyTimePatternCnt)
    {
        this.patternsName = patternsName;
        this.interval = interval;
        this.princessCoolTime = princessCoolTime;
        this.buffDuration = buffDuration;
        this.earlyTimeInterval = earlyTimeInterval;
        this.earlyTimePatternCnt = earlyTimePatternCnt;
    }
    public string[] patternsName;
    public float interval;
    public float princessCoolTime;
    public float buffDuration;
    public float earlyTimeInterval;
    public float earlyTimePatternCnt;
}
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


    public JsonData objectData { get; private set; }
    public JsonData stageData { get; private set; }
    public JsonData stageDesignData { get; private set; }
    public JsonData princessScript { get; private set; }
    public JsonData chapterData { get; private set; }


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

    public string GetKoreanName(string name)
    {
        return objectData[name]["Korean Name"].ToString();
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

    public StagePattern GetStagePattern(string stage)
    {
        //string[] patternsName = (string[])stageDesignData[stage]["EnemyPatttern"];
        List<string> patternList = new List<string>();
        
        for(int i = 0;i<stageDesignData[stage]["EnemyPattern"].Count;i++)
        {
            patternList.Add(stageDesignData[stage]["EnemyPattern"][i].ToString());
        }

        float interval = float.Parse(stageDesignData[stage]["Interval"].ToString());
        float princessCoolTime = float.Parse(stageDesignData[stage]["PrincessCoolTime"].ToString());
        float buffDuration = float.Parse(stageDesignData[stage]["BuffDuration"].ToString());
        float earlyTimeInterval = float.Parse(stageDesignData[stage]["EarlyTimeInterval"].ToString());
        float earlyTimePatternCnt = float.Parse(stageDesignData[stage]["EarlyTimePatternCnt"].ToString());

        return new StagePattern(patternList.ToArray(), interval, princessCoolTime, buffDuration,earlyTimeInterval,earlyTimePatternCnt);
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