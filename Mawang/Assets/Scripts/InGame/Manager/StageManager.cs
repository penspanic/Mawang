using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class StageManager : MonoBehaviour
{

    private JsonData stageData;
    private TextAsset stageText;
    private GameManager gameMgr;
    private PrincessManager princessMgr;
    private SpriteOrderLayerManager orderMgr;
    private string stage;


    private List<GameObject> stagePatternList   =   new List<GameObject>();
    private float unitSpawnInterval;


    void Awake()
    {
        stageText = Resources.Load<TextAsset>("TextFile/NewStage Design");
        orderMgr    =   GetComponent<SpriteOrderLayerManager>();
    }

    public void Init()
    {
        princessMgr = GetComponent<PrincessManager>();
        LoadStage();

        StartCoroutine(StageSpawnLoop());
    }

    void LoadStage()
    {
        // TEMP 
        PlayerData.instance.selectedStage = "C0S1";


        stage = PlayerData.instance.selectedStage;

        // json 데이타 찾기
        stageData = JsonMapper.ToObject(stageText.text);
        JsonData patternArr = stageData[stage]["EnemyPattern"];

        //#region Princess Init

        //// 현재 공주
        //princessMgr.currPrincss = stageData[stage]["Princess"].ToString();

        //// princessMgr 이 string에 따라 INIT
        //princessMgr.InitUI();

        //#endregion

        double temp = (double)stageData[stage]["Interval"];
        unitSpawnInterval = (float)temp;
        Debug.Log(temp);
        

        // 적군 패턴 갖고오기
        for (int i = 0; i < patternArr.Count; i++)
        {
            stagePatternList.Add(
                Resources.Load<GameObject>("Prefabs/Enemy Pattern/" + patternArr[i].ToString()));
        }

    }

    IEnumerator StageSpawnLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(unitSpawnInterval);
            SpawnPattern();
        }
    }


    int rand        =   0;
    int randLine    =   0;
    void SpawnPattern()
    {
        
        // pattern
        rand    =   Random.Range(0,stagePatternList.Count);

        // 3마리가 동시에 출현할때 앞에 1_을 붙인다.
        if (stagePatternList[rand].name[0] == '1')
            randLine        =   1;
        else
            randLine        =   Random.Range(1,4);


        float randPosY      =   (randLine - 1) * -1.2f;

        Instantiate(stagePatternList[rand],new Vector3(19,randPosY,0),new Quaternion());
        orderMgr.UpdateOrder(randLine);
        //patternPrefabList.Add();
    }
}
