using UnityEngine;
using System.Collections;

/// <summary>
/// 메딕
/// ------적-------아군----메딕-------
/// 아군과 일정 거리 유지 ( idle ) 
/// 아군이 피해를 입을 경우, 즉시 힐 시작
/// 아군이 죽을경우 자리에 그자리에 서기 ( idle ) 
/// 
/// /// </summary>
public class Medic : Movable
{
    private Vector3 prevPos;

    protected override void Attack()
    {
        if (transform.position == prevPos)
            return;


        prevPos = transform.position;
        animator.Play("Attack", 0);
    }

    protected override void FindTarget()
    {
        ObjectBase[] targets = battleMgr.GetTargets(this, attackRange, canHitNum, true);

        if (targets == null)
            targets = battleMgr.GetTargets(this, attackRange, canHitNum);

        if (isSkillMotion)
        {
            state = MovableState.Skill;
            return;
        }

        if (targets == null)
            state = MovableState.Advance;
        else
        {
            if (canAttack)
                state = MovableState.Attack;
            else
                state = MovableState.Idle;
        }
    }
}
