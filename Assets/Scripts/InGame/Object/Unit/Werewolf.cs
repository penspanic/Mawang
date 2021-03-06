﻿using System.Collections;
using UnityEngine;

public class Werewolf : Warrior, ITouchable
{
    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float jumpTime;

    [SerializeField]
    private float jumpDis;

    private Transform enemyCastlePos;
    private bool isEndPos = false;
    private float fixedX = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        if (forDecoration)
            return;
        enemyCastlePos = battleMgr.enemyCastle.transform;
    }

    // Mathf.Abs(2 *  sin(x))
    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            StartCoroutine(WerewolfSkill());
        }
    }

    // skill
    private IEnumerator WerewolfSkill()
    {
        float currTime = 0.0f;
        Vector3 orginPos = transform.position;
        Vector3 jumpVec = Vector3.zero;

        EffectManager.instance.PlayEffect(EffectKind.Werewolf_Skill, transform.position + new Vector3(0.2f,0,0));

        while (currTime < jumpTime)
        {
            currTime += Time.deltaTime;

            float jumpX = (currTime / jumpTime) * (jumpDis * 3);
            // 만약 x가 적 성을 넘어가면 못넘어가게

            if (!isEndPos)
                jumpVec.x = jumpX;
            else
                jumpVec.x = fixedX;

            jumpVec.y = Mathf.Abs(jumpHeight * Mathf.Sin(jumpX / jumpDis));

            if (transform.position.x >= enemyCastlePos.position.x && !isEndPos)
            {
                fixedX = jumpVec.x;
                isEndPos = true;
            }
            transform.position = orginPos + jumpVec;
            yield return null;
        }

        if (isEndPos)
            fixedX = 0.0f;

        transform.position = new Vector2(transform.position.x, orginPos.y);
    }

    protected override void JudgmentAttack()
    {
        base.JudgmentAttack();
        EffectManager.instance.PlayEffect(EffectKind.Werewolf_Attack, transform.position + new Vector3(0.3f,0,0));
    }
}