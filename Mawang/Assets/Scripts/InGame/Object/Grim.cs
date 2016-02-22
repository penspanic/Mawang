using UnityEngine;
using System.Collections;

public class Grim : Warrior, ITouchable
{

    GameObject attackEffect;
    GameObject skillProjectile;

    protected override void Awake()
    {
        base.Awake();
        if (forDecoration)
            return;
        canUseSkill     =   true;
        attackEffect    =   transform.FindChild("AttackEffect").gameObject;
        skillProjectile =   transform.FindChild("SkillProjectile").gameObject;
    }

    protected override void Attack()
    {
        base.Attack();

        
    }
    public void OnTouch()
    {
        if (state != MovableState.Advance && canUseSkill && !isDestroyed)
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
        attackEffect.SetActive(true);
    }
    
}
