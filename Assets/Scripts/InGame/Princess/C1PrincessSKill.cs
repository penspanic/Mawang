using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 아군 적군 모두 체력 전체 회복
public sealed class C1PrincessSkill : PrincessSkillBase
{
    List<Movable> targetUnits;

    protected override void Awake()
    {
        base.Awake();
        effectColor = new Color(0.76f, 1, 0.73f, 1);
    }

    protected override IEnumerator SkillStart()
    {
        targetUnits = new List<Movable>();

        for (int i = 0; i < battleMgr.ourForceList.Count; ++i)
            targetUnits.Add(battleMgr.ourForceList[i] as Movable);

        for (int i = 0; i < battleMgr.enemyList.Count; ++i)
            targetUnits.Add(battleMgr.enemyList[i] as Movable);

        SetEffectColor(true, targetUnits);

        for (int i = 0; i < targetUnits.Count; ++i)
            targetUnits[i].SetHP(targetUnits[i].maxHP);

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        SetEffectColor(false, targetUnits);

        yield break;
    }
}