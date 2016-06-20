using UnityEngine;
using System.Collections;

public class GoldUpgradeItem : ItemBase
{
    [SerializeField]
    private float multipleGoldUpgrade;

    private GoldManager goldMgr;
    private int itemGoldIncrement;
    private const float goldIncrementDuration = 15;
    

    protected override void Awake()
    {
        base.Awake();
        goldMgr = FindObjectOfType<GoldManager>();

        coolTime = 60;
        message = "골드 획득량이 일정시간동안 증가합니다";
        itemGoldIncrement = 0;
    }

    protected override void Useitem()
    {
        if (isUsing)
        {
            msgBox.PushMessage("아직 사용할 수 없습니다.");
        }
        else
        {
            msgBox.PushMessage(message);

            --amount;
            isUsing = true;
            PlayerData.instance.UseItem(name);

            StartCoroutine(GoldUpgradeProcess());
        }
    }

    private IEnumerator GoldUpgradeProcess()
    {
        StartCoroutine(CoolTimeProcess());

        itemGoldIncrement = (int)(goldMgr.goldIncreaseAmount * multipleGoldUpgrade);
        itemGoldIncrement -= goldMgr.goldIncreaseAmount;

        goldMgr.goldIncreaseAmount += itemGoldIncrement;

        yield return new WaitForSeconds(goldIncrementDuration);

        goldMgr.goldIncreaseAmount -= itemGoldIncrement;
    }
}