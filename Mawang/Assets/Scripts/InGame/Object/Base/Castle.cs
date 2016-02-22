using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : ObjectBase
{
    private SpriteRenderer front;
    [SerializeField] private Image      hpBar;


    GameManager gameMgr;

    protected SpriteRenderer spr;

    protected override void Awake()
    {
        base.Awake();
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        front   =   transform.FindChild("Front").GetComponent<SpriteRenderer>();
        spr     =   this.GetComponent<SpriteRenderer>();
        StartCoroutine(CastleProcess());
    }
    protected IEnumerator CastleProcess()
    {
        while(true)
        {
            ValueUpdate(hp, maxHP);
            yield return null;
        }
    }

    public override ObjectBase[] GetTargets()
    {
        return battleMgr.GetTargets(this, this.attackRange, this.canHitNum);
    }

    private void ValueUpdate(float currValue, float maxValue)
    {
        hpBar.fillAmount = currValue / maxValue;
    }

    public override void Attacked(int damage)
    {
        damage = ApplyDefensive(damage);
        hp -= damage;
        if (hp <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            gameMgr.CastleDestroyed(this);
        }

        if(!isBleed)
            StartCoroutine(ChangeDamageColor());
    }


    protected override IEnumerator ChangeDamageColor()
    {
        isBleed  =   true;

        // 성 앞부분이 빨갛게 안되서.
        spr.color   = Color.red;
        front.color = Color.red;

        yield return new WaitForSeconds(damagedColorDuration);

        spr.color   = Color.white;
        front.color = Color.white;

        isBleed  =   false;
    }



}
