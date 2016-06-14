using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 적군 체력 회복 / 공속 증가
public sealed class C4PrincessSkill : PrincessSkillBase
{
    List<Movable> targetUnits;
    const int ASIncreasePercent = 20;
    const float HPHealRatio = 0.5f;

    protected override IEnumerator SkillStart()
    {
        targetUnits = new List<Movable>();

        for (int i = 0; i < battleMgr.enemyList.Count; ++i)
        {
            targetUnits.Add(battleMgr.enemyList[i] as Movable);
        }

        SetEffectColor(true, targetUnits);

        for (int i = 0; i < targetUnits.Count; ++i)
        {
            targetUnits[i].AddAttackSpeed(ASIncreasePercent);

            int newHP = targetUnits[i].GetHP() + (int)((float)targetUnits[i].maxHP * HPHealRatio);
            targetUnits[i].SetHP(newHP);
        }

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        for (int i = 0; i < targetUnits.Count; ++i)
        {
            if (targetUnits[i] != null)
                targetUnits[i].AddAttackSpeed(-ASIncreasePercent);
        }

        yield break;
    }
}