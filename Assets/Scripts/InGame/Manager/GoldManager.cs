using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    private int goldUpgradeCost = 150;
    private int goldUpgradeAddCost = 40;

    private int goldIncreaseAmount = 10; // 기본 초당 골드 습득량
    private int goldUpgradeAddAmount = 5; // 업그레이드할 때마다 추가 골드 습득량
    private int maxGoldAddAmount = 120; // 업그레이드할 때마다 추가 최대 골드량
    private int playerMaxGold = 200;

    public static readonly int MaxUpgradeStep = 5;

    public int playerGold;

    private int goldUpgradeStep;

    private GameManager gameMgr;
    private Text playerGoldText;
    private Text upgradeCostText;

    private void Awake()
    {
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        playerGoldText = GameObject.Find("Gold Text").GetComponent<Text>();
        upgradeCostText = GameObject.Find("GoldUpgradeCost").GetComponent<Text>();
        StartCoroutine(UpdateGoldText());
        StartCoroutine(IncreaseGold());
        SetUpgradeCostText();
    }

    private void Start()
    {
        CheckChapter();
        SetUpgradeCostText();
    }

    private void CheckChapter()
    {
        if (PlayerData.instance.GetSelectedChapter() >= 4)
        {
            playerMaxGold       = 500;  // 기본 최대 골드량
            goldUpgradeCost     = 350;  // 기본 업그레이드 코스트
            goldIncreaseAmount  = 20;   // 기본 초당 골드 습득량


            maxGoldAddAmount    = 150;   // 추가 최대 골드량
            goldUpgradeAddCost  = 100;   // 추가 코스트 증가
            goldUpgradeAddAmount = 10;   // 추가 골드 습득량

            playerGold = 400;
        }
    }

    private IEnumerator UpdateGoldText()
    {
        while (gameMgr.isRun)
        {
            SetGoldText();
            yield return null;
        }
    }

    private IEnumerator IncreaseGold()
    {
        while (gameMgr.isRun)
        {
            while (TutorialManager.instance.isPlaying)
                yield return null;

            yield return new WaitForSeconds(1f);
            if (playerGold + goldIncreaseAmount > playerMaxGold)
            {
                playerGold = playerMaxGold;
            }
            else
            {
                playerGold += goldIncreaseAmount;
            }
        }
    }

    private void SetGoldText()
    {
        playerGoldText.text = playerGold.ToString();
    }

    private void SetUpgradeCostText()
    {
        upgradeCostText.text = goldUpgradeCost.ToString();

        if (goldUpgradeStep == MaxUpgradeStep)
            upgradeCostText.text = "Max";
    }

    public void GoldUpgrade()
    {
        playerGold -= goldUpgradeCost;
        goldIncreaseAmount += goldUpgradeAddAmount;
        playerMaxGold += maxGoldAddAmount;
        goldUpgradeCost += goldUpgradeAddCost;

        goldUpgradeStep++;
        SetUpgradeCostText();
    }

    public bool CanGoldUpgrade()
    {
        if (playerGold - goldUpgradeCost < 0)
            return false;
        return true;
    }

    public int GetGoldUpgradeStep()
    {
        return goldUpgradeStep;
    }

    public void AddGold(int gold)
    {
        if (playerGold + gold > playerMaxGold)
            playerGold = playerMaxGold;
        else
            playerGold += gold;
    }
}