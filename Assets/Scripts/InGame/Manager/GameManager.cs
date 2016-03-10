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


    //  TutorialManager tutorialMgr;
    BgmManager      bgmMgr;
    SpriteOrderLayerManager orderMgr;
    Pause pauseUI;
    

    void Awake()
    {
        // tutorialMgr     =   GameObject.FindObjectOfType<TutorialManager>();
        orderMgr        =   FindObjectOfType<SpriteOrderLayerManager>();
        isRun           =   true;
        pauseUI         =   FindObjectOfType<Pause>();

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

        Debug.Log("Destroyed");
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
