using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private Text    playerMoneyText;
    public bool    isOver              = false;


    StageManager    stageMgr;
    TutorialManager tutorialMgr;
    BgmManager      bgmMgr;
    // Event
    public GameObject gameOver;
    public GameObject gameClear;

    void Awake()
    {
        playerMoneyText =   GameObject.Find("MoneyText").GetComponent<Text>();
        stageMgr        =   GameObject.FindGameObjectWithTag("Manager").GetComponent<StageManager>();
        tutorialMgr     =   GameObject.FindGameObjectWithTag("Manager").GetComponent<TutorialManager>();
    }

    void Start()
    {
        stageMgr.Init();
        if(PlayerData.instance.selectedStage == "C0S1")
            tutorialMgr.Init();
            
    }
}
