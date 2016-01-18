using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int playerMoney
    {
        get;
        set;
    }
    public int playerMaxMoney
    {
        get;
        set;
    }
    
    private Text    playerMoneyText;
    private bool    isOver              = false;
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
        // 얘도 플레이어데이터에서 대리고와서 해야됨.() 지금은 걍 박음
        playerMaxMoney = 500;
        playerMoney = 0;
    }

    void Start()
    {
        stageMgr.Init();
        if(PlayerData.instance.selectedStage == "C0S1")
            tutorialMgr.Init();
            
        StartCoroutine(GameLoop());
        StartCoroutine(IncreaseMoney());
    }



    private IEnumerator GameLoop()
    {
        while (!isOver)
        {

            // ShowMoney
            ShowMoney();

            // Princess

            yield return null;
        }
    }


    #region Money

    private void ShowMoney()
    {
        playerMoneyText.text = ((int)playerMoney).ToString();
    }
    private IEnumerator IncreaseMoney()
    {
        while (!isOver)
        {
            if (playerMoney + moneyIncreaseAmount > playerMaxMoney)
                playerMoney = playerMaxMoney;
            else
                playerMoney += moneyIncreaseAmount;
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion




    public void AddMoney(int addMoney)
    {
        if (addMoney + playerMoney >= playerMaxMoney)
            playerMoney     =   playerMaxMoney;
        else
            playerMoney += addMoney;
    }


    public void AddMoneyIncrease(int add)
    {
        moneyIncreaseAmount += add;
    }
}
