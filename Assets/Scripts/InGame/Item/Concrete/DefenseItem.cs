using System.Collections;
using UnityEngine;

public class DefenseItem : ItemBase
{
    private SatanCastle satanCastle;
    private const int durationTime = 10;
    private const float defensivePower = 1f;

    private Coroutine effectCoroutine;

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
        if (isUsing) // 사용중일 때에는 지속시간만 초기화
        {
            msgBox.PushMessage("지속시간이 초기화되었습니다.");
            StopCoroutine(effectCoroutine);
            effectCoroutine = StartCoroutine(ItemEffect());
        }
        else
        {
            msgBox.PushMessage(message);
            effectCoroutine = StartCoroutine(ItemEffect());
        }
    }

    private IEnumerator ItemEffect()
    {
        isUsing = true;
        satanCastle.SetDefensivePower(defensivePower);

        yield return new WaitForSeconds(durationTime);

        satanCastle.SetDefensivePower(0);
        isUsing = false;
    }
}