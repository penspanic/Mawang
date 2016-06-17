using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FreezeItem : ItemBase
{
    private BattleManager battleMgr;
    private int duration = 5;
    public readonly static Color freezedColor = new Color(58f / 255f, 215f / 255f, 259f / 255f);
    private Image skillImg;

    protected override void Awake()
    {
        base.Awake();
        battleMgr = GameObject.FindObjectOfType<BattleManager>();
        coolTime = 40;
        skillImg = transform.FindChild("Freeze Image").GetComponent<Image>();
        message = "적 유닛의 이동을 막습니다.";
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

            amount--;
            isUsing = true;
            PlayerData.instance.UseItem(name);

            StartCoroutine(FreezeProcess());
        }
    }

    private IEnumerator FreezeProcess()
    {

        StartCoroutine(CoolTimeProcess());

        // 얼릴 적 찾기
        Movable[] enemys = System.Array.FindAll<Movable>
            (GameObject.FindObjectsOfType<Movable>(), (obj) =>
            {
                return obj.CompareTag("Enemy") && !obj.isDestroyed;
            });

        for (int i = 0; i < enemys.Length; ++i)
            enemys[i].Freeze(true);

        yield return new WaitForSeconds(duration);

        // 죽은 것 골라내기 (null 이거나 isDestroyed가 참일 때)

        enemys = System.Array.FindAll<Movable>(enemys, (obj) =>
        {
            if (obj == null)
                return false;
            else
                return !obj.isDestroyed;
        });

        for (int i = 0; i < enemys.Length; ++i)
            enemys[i].Freeze(false);
    }
}