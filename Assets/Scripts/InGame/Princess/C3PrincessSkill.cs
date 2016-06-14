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

        for (int i = 0; i < targetUnits.Count; ++i)
            targetUnits.Add(targetUnits[i]);

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