using UnityEngine;
using System.Collections;

public class DefenseItem : ItemBase
{

    SatanCastle satanCastle;
    int durationTime = 60;
    float defensivePower = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        satanCastle = GameObject.FindObjectOfType<SatanCastle>();
        message = "1분간 방어태세에 돌입합니다!";
    }

    protected override void Useitem()
    {
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
