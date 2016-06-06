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
        foreach (Movable eachUnit in battleMgr.enemyList)
            targetUnits.Add(eachUnit);

        SetEffectColor(true, targetUnits);

        foreach (Movable eachUnit in targetUnits)
        {
            eachUnit.AddAttackSpeed(ASIncreasePercent);
            eachUnit.SetMinusHP(-(int)((float)eachUnit.maxHP * HPHealRatio));
        }

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        foreach(Movable eachUnit in targetUnits)
        {
            if (eachUnit != null)
                eachUnit.AddAttackSpeed(-ASIncreasePercent);
        }

        yield break;
    }
}