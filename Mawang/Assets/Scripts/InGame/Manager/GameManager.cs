using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private Text    playerMoneyText;
    public bool    isRun              = false;

    public GameObject gameOver;
    public GameObject gameClear;

    public SatanCastle satanCastle;

    StageManager    stageMgr;
    TutorialManager tutorialMgr;
    BgmManager      bgmMgr;

    

    void Awake()
    {
        playerMoneyText =   GameObject.Find("MoneyText").GetComponent<Text>();
        stageMgr        =   GameObject.FindObjectOfType<StageManager>();
        tutorialMgr     =   GameObject.FindObjectOfType<TutorialManager>();

        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        GameObject.FindObjectOfType<SceneFader>().transform.SetParent(Camera.main.transform, true);
    }

    void Start()
    {
        stageMgr.Init();
        if(PlayerData.instance.selectedStage == "C0S1")
            tutorialMgr.Init();
        
       
    }

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
