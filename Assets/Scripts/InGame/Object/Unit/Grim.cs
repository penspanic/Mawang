﻿using UnityEngine;

public class Grim : Warrior, ITouchable
{
    private GameObject skillProjectile;

    protected override void Awake()
    {
        base.Awake();
        skillProjectile = transform.FindChild("SkillProjectile").gameObject;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    public void OnTouch()
    {
        if (/*state != MovableState.Advance && */canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
        }
    }

    public void OnSkillMotionEvent()
    {
        skillProjectile.SetActive(true);
        SkillMotionEnd();
    }

    protected override void JudgmentAttack()
    {
        base.JudgmentAttack();

        Vector3 spawnPos = transform.position;
        spawnPos += new Vector3(1.1f, -0.4f, 0);

        EffectManager.instance.PlayEffect(EffectKind.Grim_attack, spawnPos);
    }
}