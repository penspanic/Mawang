using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 아군 유닛 체력 회복
public sealed class C3PrincessSkill : PrincessSkillBase
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
        foreach (Movable eachUnit in battleMgr.ourForceList)
            targetUnits.Add(eachUnit);

        SetEffectColor(true, targetUnits);

        foreach(Movable eachUnit in targetUnits)
        {
            eachUnit.SetFullHP();
        }

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        SetEffectColor(false, targetUnits);

        yield break;
    }
}