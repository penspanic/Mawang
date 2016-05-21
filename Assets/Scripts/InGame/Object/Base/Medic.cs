using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private int healPerSec;
    [SerializeField]
    private bool isOverlapHeal;
    private List<ObjectBase> healTargets;

    private bool isHealing = false;
    private bool onceAttackAni = true;

    protected override void Awake()
    {
        healTargets = new List<ObjectBase>();
        base.Awake();
    }

    protected override void Attack()
    {
        if (!isHealing)
        {
            if (onceAttackAni)
            {
                animator.Play("Attack", 0);
                onceAttackAni = false;
            }
        }
    }

    public void OnAttackEnd()
    {
        AttackEnd();

        if (healTargets.Count > 0)
            StartCoroutine(HealProcess(healTargets[0]));
    }

    private IEnumerator HealProcess(ObjectBase healTarget)
    {
        while (!healTarget.isDestroyed
            && Mathf.Abs(healTarget.transform.position.x - transform.position.x) <= attackRange * BattleManager.fightDistance
            && healTarget.GetHP() != healTarget.maxHP)
        {
            isHealing = true;
            yield return new WaitForSeconds(1f);

            // 단일
            healTarget.SetMinusHP(-healPerSec);
        }
        onceAttackAni = true;
        isHealing = false;
    }

    protected override void FindTarget()
    {
        if (isHealing)
            return;

        // 인접 아군 유닛 찾음
        ObjectBase[] targets = battleMgr.GetTargets(this, attackRange, canHitNum, true);

        // 힐 받을 유닛 초기화
        healTargets.Clear();
        if (targets == null)
        {
            targets = battleMgr.GetTargets(this, attackRange, canHitNum);

        }
        else
        {
            for (int i = 0; i < targets.Length; ++i)
            {
                if (targets[i].GetHP() < targets[i].maxHP && !targets[i].isOurForce)
                {
                    healTargets.Add(targets[i]);
                }
            }
        }

        if (healTargets.Count == 0)
            canAttack = false;

        if (isOverlapHeal)
        {
            if (IsOverlapMedic(targets))
            {
                state = MovableState.Advance;
                return;
            }

        }


        // 행동 분배
        if(targets != null)
            Debug.Log("targets: " + targets.Length);
        // 아군도 적군도 없을 경우
        if (targets == null || targets.Length == 0)
            state = MovableState.Advance;
        else
        {
            // 공격이 가능한 경우
            if (canAttack)
                state = MovableState.Attack;
            else // 공격이 불가능한 경우
                state = MovableState.Idle;
        }
    }

    private bool IsOverlapMedic(ObjectBase[] targets)
    {
        if (targets == null)
            return false;

        if (targets[0].name.Contains("Medic"))
            return true;

        return false;
    }
}