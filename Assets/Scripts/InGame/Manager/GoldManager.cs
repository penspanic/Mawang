using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldManager : MonoBehaviour
{
    private int goldUpgradeCost = 200; 
    private int goldUpgradeAddCost = 100;

    private int goldIncreaseAmount = 10; // 기본 초당 골드 습득량
    private int goldUpgradeAddAmount = 10; // 업그레이드할 때마다 추가 골드 습득량
    private int maxGoldAddAmount = 100; // 업그레이드할 때마다 추가 최대 골드량
    private int playerMaxGold = 300;

    public static readonly int MaxUpgradeStep = 5;

    public int playerGold;

    int goldUpgradeStep;

    GameManager gameMgr;
    Text playerGoldText;
    Text upgradeCostText;
    void Awake()
    {
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        playerGoldText = GameObject.Find("Gold Text").GetComponent<Text>();
        upgradeCostText = GameObject.Find("GoldUpgradeCost").GetComponent<Text>();
        StartCoroutine(UpdateGoldText());
        StartCoroutine(IncreaseGold());
        SetUpgradeCostText();
    }

    IEnumerator UpdateGoldText()
    {
        while(gameMgr.isRun)
        {
            SetGoldText();
            yield return null;
        }
    }
    IEnumerator IncreaseGold()
    {
        while (gameMgr.isRun)
        {
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
    void SetGoldText()
    {
        playerGoldText.text = playerGold.ToString();
    }
    void SetUpgradeCostText()
    {
        upgradeCostText.text = goldUpgradeCost.ToString();
    }
    public void GoldUpgrade()
    {
        playerGold -= goldUpgradeCost;
        goldIncreaseAmount += goldUpgradeAddAmount;
        playerMaxGold += maxGoldAddAmount;
        goldUpgradeCost += goldUpgradeAddCost;
        
        goldUpgradeStep++;
        SetUpgradeCostText();
        Debug.Log("초당 골드 습득량 : " + goldIncreaseAmount);
        Debug.Log("최대 골드량 :" + playerMaxGold);
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
