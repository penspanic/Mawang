using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FreezeItem : ItemBase
{
    BattleManager battleMgr;
    int duration = 5;
    public static Color freezedColor = new Color(58f / 255f, 215f / 255f, 259f / 255f);
    private Image skillImg;

    protected override void Awake()
    {
        base.Awake();
        battleMgr = GameObject.FindObjectOfType<BattleManager>();
        coolTime = 40;
        skillImg = GetComponent<Image>();
        message = "적 유닛의 이동을 막습니다.";
    }

    protected override void Useitem()
    {
        if(isUsing)
        {
            msgBox.PushMessage("아직 사용할 수 없습니다.");
        }
        else
        {
            isUsing = true;
            amount--;
            msgBox.PushMessage(message);
            StartCoroutine(FreezeProcess());
        }
    }
    
    IEnumerator FreezeProcess()
    {
        StartCoroutine(CoolTiming());

        // 얼릴 적 찾기
        Movable[] enemys = System.Array.FindAll<Movable>
            (GameObject.FindObjectsOfType<Movable>(),(obj)=>{
                return obj.CompareTag("Enemy");
            });

        for (int i = 0; i < enemys.Length;i++)
            enemys[i].Freeze(true);

        yield return new WaitForSeconds(duration);

        // 죽은 것 골라내기 (null 이거나 isDestroyed가 참일 때)

        enemys = System.Array.FindAll<Movable>(enemys, (obj) =>
        {
            if(obj == null)
                return false;
            else
                return !obj.isDestroyed;
        });

        for (int i = 0; i < enemys.Length; i++)
            enemys[i].Freeze(false);

        
    }

    IEnumerator CoolTiming()
    {
        float currTime = 0.0f;

        while (currTime < coolTime)
        {
            currTime += Time.deltaTime;

            skillImg.fillAmount =  currTime / coolTime;
            yield return null;
        }

        isUsing = false;
    }
}
