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
    private float unitSpawnEarlyTime; // 초반 약할때 추가시간
    private float earlyTimePatternCnt; // 약할때 추가시간 패턴 갯수

    private string stage;
    private bool   isDefenceTurn;

    BgmManager      bgmMgr;
    SpriteOrderLayerManager orderMgr;
    BattleManager   battleMgr;
    Pause pauseUI;
    

    void Awake()
    {
        // tutorialMgr     =   GameObject.FindObjectOfType<TutorialManager>();
        orderMgr        =   FindObjectOfType<SpriteOrderLayerManager>();
        battleMgr       =   FindObjectOfType<BattleManager>();
        isRun           =   true;
        pauseUI         =   FindObjectOfType<Pause>();
        isDefenceTurn   =   true;

        PlayerData.instance.CheckInstance();
        GameEventManager.instance.CheckInstance();

        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        GameObject.FindObjectOfType<SceneFader>().transform.SetParent(Camera.main.transform, true);


    }

    void Start()
    {
        LoadStage();
        if (PlayerData.instance.selectedStage == "C0S1")
        {
            Time.timeScale = 0;
            TutorialManager.Instance.PlayTutorial(TutorialEvent.PrepareGame);
        }


        StartCoroutine(StageSpawnLoop());
    }

    #region stage
    void LoadStage()
    {
        stage = PlayerData.instance.selectedStage;

        StagePattern pattern = JsonManager.instance.GetStagePattern(stage);


        unitSpawnInterval = pattern.interval;
        unitSpawnEarlyTime = pattern.earlyTimeInterval;
        earlyTimePatternCnt = pattern.earlyTimePatternCnt;

        if (stage == "C0S1")
            unitSpawnInterval = 2.5f;

        // 적군 패턴 갖고오기
        for (int i = 0; i < pattern.patternsName.Length; i++)
        {
            stagePatternList.Add(
                Resources.Load<GameObject>("Prefabs/Enemy Pattern/" + stage[1] + "/" + pattern.patternsName[i]));
        }

    }

    IEnumerator StageSpawnLoop()
    {
        while (true)
        {
            #region tutorial

            if (PlayerData.instance.selectedStage == "C0S1")
            {
                if (TutorialManager.Instance.PatternCnt == 0)
                    unitSpawnInterval = JsonManager.instance.GetStagePattern("C0S1").interval;
                else
                    TutorialManager.Instance.PatternCnt--;
            }

            #endregion

            Debug.Log(unitSpawnEarlyTime);
            Debug.Log(unitSpawnInterval);
            if (earlyTimePatternCnt > 0)
                earlyTimePatternCnt--;
            else
                unitSpawnEarlyTime = 0;

            Debug.Log("interval : " + unitSpawnInterval + unitSpawnEarlyTime);
            yield return new WaitForSeconds(unitSpawnInterval + unitSpawnEarlyTime);
            
            SpawnPattern();
        }
    }


    int rand = 0;
    int prevRand = 99;

    int randLine = 0;
    int prevLine = 99;
    int prev2Line = 99;
    float spawnLine = 1;
    void SpawnPattern()
    {

        // 첨엔 무조건 첫번째꺼 소환
        // pattern
        while (prevRand == rand)
        {
            rand = Random.Range(0, stagePatternList.Count);

            if (stagePatternList.Count == 1)
                break;
        }

        prevRand = rand;

        // 3마리가 동시에 출현할때 앞에 1_을 붙인다.
        if (stagePatternList[rand].name[0] == '1')
        {
            randLine = 1;
            isDefenceTurn = false;
        }
        else
        {
            randLine = Random.Range(1, 4);

            while (randLine == prevLine || randLine == prev2Line)
                randLine = Random.Range(1, 4);


            if (isDefenceTurn)
            {
                float minPos = 100f;
                for (int i = 0; i < battleMgr.ourForceList.Count; i++)
                {
                    if (battleMgr.enemyCastle.transform.position.x - battleMgr.ourForceList[i].transform.position.x < minPos)
                    {
                        minPos = battleMgr.enemyCastle.transform.position.x - battleMgr.ourForceList[i].transform.position.x;
                        spawnLine = battleMgr.ourForceList[i].line;
                    }
                }
            }
        }


        prev2Line = prevLine;
        prevLine = randLine;

        float randPosY = ((isDefenceTurn ? spawnLine-1 : randLine - 1)) * -1.2f;
        Debug.Log(isDefenceTurn);
        Instantiate(stagePatternList[rand], new Vector3(19, randPosY, 0), new Quaternion());
        orderMgr.UpdateOrder(randLine);

        isDefenceTurn = !isDefenceTurn;
    }
    #endregion

    

    void GameClear()
    {
        gameClear.SetActive(true);
        if (PlayerData.instance.lastClearedStage == null)
            GameEventManager.instance.PushEvent(GameEvent.FirstC0S1Cleared);

        if (PlayerData.instance.selectedStage == "C0S3" && PlayerData.instance.IsStageCleared("C0S3"))
            GameEventManager.instance.PushEvent(GameEvent.FirstChapter0Cleared);

        PlayerData.instance.StageClear(PlayerData.instance.selectedStage);
        StartCoroutine(TouchToMain());
    }

    void GameOver()
    {
        gameOver.SetActive(true);
        StartCoroutine(TouchToMain());
    }


    IEnumerator TouchToMain()
    {
        // 터치 대기시간 
        float currTime = 0.0f;
        float waitTime = 1;
        while(currTime <= waitTime)
        {
            currTime += Time.unscaledDeltaTime;
            yield return null;
        }
        while(true)
        {
            if (Input.GetMouseButtonDown(0))
                break;
            yield return null;
        }
        Time.timeScale = 1f;
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "Main"));
    }

    public void CastleDestroyed(Castle castle)
    {
        if (gameClear.activeSelf || gameOver.activeSelf)
            return;
        if (GameObject.FindObjectOfType<Pause>().isSceneChanging)
            return;

        isRun = false;
        Time.timeScale = 0f;

        if(castle is SatanCastle) // Game Over
        {
            GameOver();
        }
        else                      // Game Clear
        {
            GameClear();
        }
    }


    public void OnApplicationPause(bool pause)
    {
        if (TutorialManager.Instance.isPlaying)
            return;

        if (pause)
        {
            pauseUI.PauseButtonDown();
        }
    }

}
