using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 천둥 번개 발생( 랜덤으로 아군, 적군 사망)
public sealed class C5PrincessSkill : PrincessSkillBase
{
    Lightnings lightnings;

    protected override void Awake()
    {
        base.Awake();
        lightnings = GameObject.FindObjectOfType<Lightnings>();
    }

    protected override IEnumerator SkillStart()
    {
        int allUnitCount = battleMgr.ourForceList.Count + battleMgr.enemyList.Count;
        int killCount = Random.Range(allUnitCount / 3, allUnitCount);

        List<ObjectBase> allUnitList = new List<ObjectBase>(battleMgr.ourForceList);
        allUnitList.AddRange(battleMgr.enemyList);

        List<Movable> killList = new List<Movable>();

        while (killList.Count < killCount)
        {
            int randomIndex = Random.Range(0, allUnitList.Count);

            if (!killList.Contains(allUnitList[randomIndex] as Movable))
                killList.Add(allUnitList[randomIndex] as Movable);
        }

        foreach (Movable eachUnit in killList)
        {
            eachUnit.isDestroyed = true;
            StartCoroutine(lightnings.ShowLightning(eachUnit.transform.position + new Vector3(0, 1, 0), "Skill Effect"));

        }

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        yield break;
    }
}