using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 공주 버프가 진행중일 때 아군들 1번 부활
public sealed class C7PrincessSkill : PrincessSkillBase
{
    List<Movable> targetUnits;
    bool skillProcessing = false;

    SpawnManager spawnMgr;
    SelectTab selectTab;

    protected override void Awake()
    {
        base.Awake();

        effectColor = new Color(249f / 255f, 158f / 255f, 235f / 255f, 1f);

        spawnMgr = GameObject.FindObjectOfType<SpawnManager>();
        selectTab = GameObject.FindObjectOfType<SelectTab>();
    }

    protected override IEnumerator SkillStart()
    {
        targetUnits = new List<Movable>();

        for (int i = 0; i < battleMgr.ourForceList.Count; ++i)
            targetUnits.Add(battleMgr.ourForceList[i] as Movable);
        SetEffectColor(true, targetUnits);

        skillProcessing = true;
        StartCoroutine(RevivalProcess());

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        skillProcessing = false;

        SetEffectColor(false, targetUnits);

        yield break;
    }

    IEnumerator RevivalProcess()
    {
        List<Movable> deadUnits = new List<Movable>();

        while(skillProcessing)
        {
            Movable eachUnit = null;
            for(int i = 0;i<targetUnits.Count;++i)
            {
                eachUnit = targetUnits[i];
                if (eachUnit.isDestroyed)
                    deadUnits.Add(eachUnit);
            }

            for(int i = 0;i<deadUnits.Count;++i)
            {
                eachUnit = deadUnits[i];
                string name = eachUnit.name.Remove(eachUnit.name.IndexOf('(')); // (Clone) 문자열 삭제
                Movable spawnTarget = selectTab.GetPrefab(name);
                if (spawnTarget != null)
                    StartCoroutine(WaitAndRevival(spawnTarget, eachUnit.line, eachUnit.transform.position));

                targetUnits.Remove(eachUnit);
            }

            deadUnits.Clear();

            yield return null;
        }
    }

    IEnumerator WaitAndRevival(Movable target, int line, Vector3 pos)
    {
        yield return new WaitForSeconds(1f);

        spawnMgr.SpawnOurForce(target, line, pos);
    }
}