using UnityEngine;
using System.Collections;

// 아군 성에 데미지
public sealed class C2PrincessSkill : PrincessSkillBase
{
    const int skillDamage = 100;
    protected override IEnumerator SkillStart()
    {
        battleMgr.ourCastle.GetComponent<SatanCastle>().Attacked(skillDamage);

        yield break;
    }

    protected override IEnumerator SkillEnd()
    {
        yield break;
    }
}