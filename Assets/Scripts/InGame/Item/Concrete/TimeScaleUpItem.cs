using UnityEngine;
using System.Collections;

public class TimeScaleUpItem : ItemBase
{
    GameManager gameMgr;
    const float durationTime = 30f;

    protected override void Awake()
    {
        base.Awake();
        coolTime = (int)durationTime + 5;

        gameMgr = GameObject.FindObjectOfType<GameManager>();
    }

    protected override void Useitem()
    {
        if(isUsing)
        {
            msgBox.PushMessage("아직 사용할 수 없습니다.");
            return;
        }
        msgBox.PushMessage("게임 진행 속도가 빨라집니다!");

        --amount;
        isUsing = true;
        PlayerData.instance.UseItem(name);

        StartCoroutine(TimeScaleUpProcess());
    }

    IEnumerator TimeScaleUpProcess()
    {
        StartCoroutine(CoolTimeProcess());

        float elaspedTIme = 0f;

        while(elaspedTIme < 5f)
        {
            elaspedTIme += Time.unscaledTime;
            Time.timeScale = Mathf.Lerp(1f, 2f, elaspedTIme);
            gameMgr.userTimeScale = Time.timeScale;
            yield return null;
        }

        elaspedTIme = 0f;
        Time.timeScale = 2f;
        gameMgr.userTimeScale = Time.timeScale;

        yield return new WaitForSeconds(durationTime);

        while (elaspedTIme < 5f)
        {
            elaspedTIme += Time.unscaledTime;
            Time.timeScale = 2f - Mathf.Lerp(0f, 1f, elaspedTIme);
            gameMgr.userTimeScale = Time.timeScale;
            yield return null;
        }

        Time.timeScale = 1f;
        gameMgr.userTimeScale = Time.timeScale;
    }
}