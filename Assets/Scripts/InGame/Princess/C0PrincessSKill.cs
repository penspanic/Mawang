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

        for (int i = 0; i < battleMgr.ourForceList.Count; ++i)
            targetUnits.Add(battleMgr.ourForceList[i] as Movable);

        SetEffectColor(true, targetUnits);

        for(int i = 0;i<targetUnits.Count;++i)
        {
            targetUnits[i].AddAttackSpeed(-ASDecreasePercent);
        }

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        SetEffectColor(false, targetUnits);

        for(int i = 0;i<targetUnits.Count;++i)
        {
            if (targetUnits[i] != null)
                targetUnits[i].AddAttackSpeed(ASDecreasePercent);
        }

        yield break;
    }
}