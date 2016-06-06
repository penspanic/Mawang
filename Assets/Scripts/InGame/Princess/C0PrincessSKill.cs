using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 아군 공속 40% 감소
public sealed class C0PrincessSkill : PrincessSkillBase
{
    List<Movable> targetUnits;
    const int ASDecreasePercent = 40;

    protected override void Awake()
    {
        base.Awake();
        effectColor = new Color(0.372f, 0.815f, 0.905f, 1f);
    }

    protected override IEnumerator SkillStart()
    {
        targetUnits = new List<Movable>();
        foreach (Movable eachUnit in battleMgr.ourForceList)
            targetUnits.Add(eachUnit);

        SetEffectColor(true, targetUnits);

        foreach(Movable eachUnit in targetUnits)
        {
            eachUnit.AddAttackSpeed(-ASDecreasePercent);
        }

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        SetEffectColor(false, targetUnits);

        foreach(Movable eachUnit in targetUnits)
        {
            if (eachUnit != null)
                eachUnit.AddAttackSpeed(ASDecreasePercent);
        }

        yield break;
    }
}