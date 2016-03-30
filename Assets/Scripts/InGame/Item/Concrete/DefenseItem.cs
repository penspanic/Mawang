using UnityEngine;
using System.Collections;

public class DefenseItem : ItemBase
{

    SatanCastle satanCastle;
    const int durationTime = 10;
    const float defensivePower = 1f;

    protected override void Awake()
    {
        base.Awake();
        satanCastle = GameObject.FindObjectOfType<SatanCastle>();
        message = "10초간 마왕성이 무적상태가 됩니다!";
    }

    protected override void Useitem()
    {
        PlayerData.instance.UseItem(name);
        amount--;
        if(isUsing) // 사용중일 때에는 지속시간만 초기화
        {
            msgBox.PushMessage("지속시간이 초기화되었습니다.");
            StopCoroutine(ItemEffect());
            StartCoroutine(ItemEffect());
        }
        else
        {
            isUsing = true;
            msgBox.PushMessage(message);
            StartCoroutine(ItemEffect());
        }
    }

    IEnumerator ItemEffect()
    {
        satanCastle.SetDefensivePower(defensivePower);
        
        yield return new WaitForSeconds(durationTime);

        satanCastle.SetDefensivePower(0);
        isUsing = false;
    }
}
