using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class GameManager : MonoBehaviour
{

    public bool isRun { get; set; }

    public GameObject gameOver;
    public GameObject gameClear;

    public SatanCastle satanCastle;

    private List<GameObject> stagePatternList = new List<GameObject>();
    private float unitSpawnInterval;
    private string stage;


    TutorialManager tutorialMgr;
    BgmManager      bgmMgr;
    SpriteOrderLayerManager orderMgr;

    

    void Awake()
    {
        tutorialMgr     =   GameObject.FindObjectOfType<TutorialManager>();
        orderMgr        =   FindObjectOfType<SpriteOrderLayerManager>();
        isRun           =   true;

        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        GameObject.FindObjectOfType<SceneFader>().transform.SetParent(Camera.main.transform, true);

    }

    void Start()
    {
        LoadStage();
        if(PlayerData.instance.selectedStage == "C0S1")
            tutorialMgr.Init();

        StartCoroutine(StageSpawnLoop());
    }

    #region stage
    void LoadStage()
    {
        // TEMP 
        PlayerData.instance.selectedStage = "C1S1";


        stage = PlayerData.instance.selectedStage;
        JsonData patternArr = JsonManager.instance.GetCurrStage()["EnemyPattern"];

        double temp = (double)JsonManager.instance.GetCurrStage()["Interval"];
        unitSpawnInterval = (float)temp;


        // 적군 패턴 갖고오기
        for (int i = 0; i < patternArr.Count; i++)
        {
            stagePatternList.Add(
                Resources.Load<GameObject>("Prefabs/Enemy Pattern/" + stage[1] + "/"+patternArr[i].ToString()));
        }

    }

    IEnumerator StageSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(unitSpawnInterval);
            SpawnPattern();
        }
    }


    int rand = 0;
    int randLine = 0;
    void SpawnPattern()
    {

        // pattern
        rand = Random.Range(0, stagePatternList.Count);

        // 3마리가 동시에 출현할때 앞에 1_을 붙인다.
        if (stagePatternList[rand].name[0] == '1')
            randLine = 1;
        else
            randLine = Random.Range(1, 4);


        float randPosY = (randLine - 1) * -1.2f;

        Instantiate(stagePatternList[rand], new Vector3(19, randPosY, 0), new Quaternion());
        orderMgr.UpdateOrder(randLine);
    }
    #endregion

    

    void GameClear()
    {
        gameClear.SetActive(true);
    }

    void GameOver()
    {
        gameOver.SetActive(true);
    }

    public void CastleDestroyed(Castle castle)
    {
        if (gameClear.activeSelf || gameOver.activeSelf)
            return;

        Debug.Log("Destroyed");
        isRun = false;
        Time.timeScale = 0f;

        if(castle is SatanCastle) // Game over
        {
            GameOver();
        }
        else                      // Game Clear
        {
            GameClear();
        }
    }

}
