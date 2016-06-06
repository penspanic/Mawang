using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 랜덤으로 아군 빙결
public sealed class C6PrincessSkill : PrincessSkillBase
{
    protected override IEnumerator SkillStart()
    {
        int freezeCount = Random.Range(0, battleMgr.ourForceList.Count);
        freezeCount /= 2;

        List<Movable> freezeList = new List<Movable>();

        while (freezeList.Count < freezeCount)
        {
            int randomIndex = Random.Range(0, battleMgr.ourForceList.Count);

            if (!freezeList.Contains(battleMgr.ourForceList[randomIndex] as Movable))
                freezeList.Add(battleMgr.ourForceList[randomIndex] as Movable);
        }

        foreach (Movable eachUnit in freezeList)
        {
            eachUnit.Freeze(true);
        }

        yield return new WaitForSeconds(5);

        foreach (Movable eachUnit in battleMgr.ourForceList)
        {
            eachUnit.Freeze(false);
        }
    }

    protected override IEnumerator SkillEnd()
    {
        yield break;
    }
}