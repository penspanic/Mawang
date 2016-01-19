using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private Text    playerMoneyText;
    public bool    isOver              = false;
    [SerializeField]  // 얘는 나중에 플레이어 데이터에서 불러온 데이터로 여기에 대입
    private int     moneyIncreaseAmount = 10;

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
            
        StartCoroutine(GameLoop());
    }



    private IEnumerator GameLoop() // 삭제해도 될듯 : 1/19 근희
    {
        while (!isOver)
        {

            yield return null;
        }
    }
}
