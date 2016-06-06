using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 공주 버프가 진행중일 때 아군들 1번 부활
public sealed class C7PrincessSkill : PrincessSkillBase
{
    List<Movable> targetUnits;
    bool skillProcessing = false;

    protected override IEnumerator SkillStart()
    {

        targetUnits = new List<Movable>();
        foreach (Movable eachUnit in battleMgr.ourForceList)
            targetUnits.Add(eachUnit);

        skillProcessing = true;
        StartCoroutine(RevivalProcess());

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        skillProcessing = false;

        yield break;
    }

    IEnumerator RevivalProcess()
    {
        while(skillProcessing)
        {
            foreach(Movable eachUnit in targetUnits)
            {
                if(eachUnit == null)
                {
                    //eachUnit.name.
                }
            }

            yield return null;
        }
    }
}