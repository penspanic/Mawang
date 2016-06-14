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
    [SerializeField]
    private GameObject healEffectPrefab;
    private GameObject healEffect;
    [SerializeField]
    private Vector2 healLocalPos;

    private List<ObjectBase> healTargets;

    private bool isHealing = false;
    private bool onceAttackAni = true;

    protected override void Awake()
    {
        healTargets = new List<ObjectBase>();
        
        base.Awake();
        healEffect = Instantiate<GameObject>(healEffectPrefab);
        healEffect.transform.SetParent(transform);
        healEffect.transform.localPosition = healLocalPos;

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
        healEffect.SetActive(true);
        while (!healTarget.isDestroyed
            && Mathf.Abs(healTarget.transform.position.x - transform.position.x) <= attackRange * BattleManager.tileSize
            && healTarget.GetHP() != healTarget.maxHP)
        {
            isHealing = true;
            yield return new WaitForSeconds(1f);

            healTarget.SetHP(healTarget.GetHP() + healPerSec);
        }
        onceAttackAni = true;
        isHealing = false;
        healEffect.SetActive(false);
    }

    private List<ObjectBase> targets = new List<ObjectBase>();
    protected override void FindTarget()
    {
        if (isHealing)
            return;

        // 힐 받을 유닛 초기화
        targets.Clear();
        healTargets.Clear();

        AddTargets(battleMgr.GetTargets(this, attackRange, canHitNum));
        AddTargets(battleMgr.GetTargets(this, attackRange, canHitNum, true));

        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; ++i)
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
            if (IsOverlapMedic(targets.ToArray()))
            {
                state = MovableState.Advance;
                return;
            }
        }



        // 아군적군 둘다 들고있음
        // * 움직일 경우 
        // 1. 타겟들이 메딕밖에 없을경우
        // 2. 타겟들이 하나도 없을경우
        // * 아닐경우
        // 1. 힐하거나
        // 2. 가만히 있음

        if (targets.Count == 0)
        {
            state = MovableState.Advance;
        }
        else
        {
            // 공격이 가능한 경우
            if (canAttack)
                state = MovableState.Attack;
            else // 공격이 불가능한 경우
                state = MovableState.Idle;
        }
    }

    public override void Freeze(bool value)
    {
        base.Freeze(value);
        healEffectPrefab.SetActive(!value);
    }
    private bool IsOverlapMedic(ObjectBase[] targets)
    {
        if (targets == null || targets.Length == 0)
            return false;

        for (int i = 0; i < targets.Length; ++i)
        {
            if (!targets[i].name.Contains("Medic"))
                return false;
        }

        return true;
    }

    private void AddTargets(ObjectBase[] targetArr)
    {
        if (targetArr != null)
        {
            targets.AddRange(targetArr);
        }
    }
}