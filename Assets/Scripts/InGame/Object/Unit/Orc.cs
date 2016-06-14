using UnityEngine;

public class Orc : Warrior, ITouchable
{
    [SerializeField]
    private int skillMultiplicationDmg;

    [SerializeField]
    private int skillHitNum;

    #region ITouchable 멤버

    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
        }
    }

    #endregion ITouchable 멤버

    public void OnSkillMotionEvent()
    {
        ObjectBase[] targets = battleMgr.GetTargets(this, attackRange, skillHitNum);
        if (targets != null)
        {
            for (int i = 0; i < targets.Length; ++i)
                targets[i].Attacked(this.attackDamage * skillMultiplicationDmg);
        }

        SkillMotionEnd();
    }

    public void OnEffect()
    {
        Vector2 spawnPos = transform.position;
        spawnPos += new Vector2(1.03f, 0.5f);
        EffectManager.instance.PlayEffect(EffectKind.Orc_skill, spawnPos);
    }
}