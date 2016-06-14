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

        ObjectBase eachObj = null;
        for(int i = 0;i<targetList.Count;++i)
        {
            eachObj = targetList[i];
            if (HitRangeIn(eachObj.transform.position.x))
                eachObj.Attacked(explosionDamage);
        }

        explosioned = true;

        OnDeathEnd();
    }

    bool HitRangeIn(float targetX)
    {
        if (Mathf.Abs(targetX - transform.position.x) < explosionRange * BattleManager.tileSize)
            return true;
        return false;
    }

    protected override void Death()
    {
        animator.enabled = false;
        OnDeathEnd();
    }
}
