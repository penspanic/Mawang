using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Aragog : Movable, ITouchable
{
    [SerializeField]
    private float explosionRange;

    private Transform enemyCastle;
    private bool explosioned;

    protected override void Awake()
    {
        base.Awake();
        if (!forDecoration)
        {
            enemyCastle = battleMgr.enemyCastle.transform;
        }
    }

    void Update()
    {
        if(!explosioned && !forDecoration)
        {
            if (transform.position.x >= enemyCastle.position.x)
                OnTouch();
        }
    }

    public void OnTouch()
    {
        //Play Effct
        EffectManager.instance.PlayEffect(EffectKind.Aragog_Effect, transform.position + new Vector3(0.5f, 1f, 0f));

        List<ObjectBase> targetList = battleMgr.GetSameLine(battleMgr.enemyList, line);
        targetList.Add(battleMgr.enemyCastle.GetComponent<ObjectBase>());

        foreach(ObjectBase eachObj in targetList)
        {
            if (Mathf.Abs(eachObj.transform.position.x - transform.position.x) < explosionRange * BattleManager.fightDistance)
                eachObj.Attacked(attackDamage);
        }
        explosioned = true;

        OnDeathEnd();
    }

    protected override void Death()
    {
        animator.enabled = false;
        OnDeathEnd();
    }
}
