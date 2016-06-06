using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Aragog : Movable, ITouchable
{
    [SerializeField]
    private float explosionRange;
    [SerializeField]
    private int explosionDamage;

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
            // 성에 닿았을 때 터진다
            if (transform.position.x >= enemyCastle.position.x) 
                OnTouch();
        }
    }

    public void OnTouch()
    {
        //Play Effct

        if (explosioned || isDestroyed)
            return;

        EffectManager.instance.PlayEffect(EffectKind.Aragog_Effect, transform.position + new Vector3(0.5f, 1f, 0f));

        List<ObjectBase> targetList = battleMgr.GetSameLine(battleMgr.enemyList, line);
        targetList.Add(battleMgr.enemyCastle.GetComponent<ObjectBase>());

        foreach(ObjectBase eachObj in targetList)
        {
            if (Mathf.Abs(eachObj.transform.position.x - transform.position.x) < explosionRange * BattleManager.tileSize)
                eachObj.Attacked(explosionDamage);
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
