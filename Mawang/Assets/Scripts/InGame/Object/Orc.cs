using UnityEngine;
using System.Collections;

public class Orc : Warrior, ITouchable
{

    [SerializeField]
    private int skillMultiplicationDmg;

    private GameObject skillEffect;
    
    protected override void Awake()
    {
        base.Awake();
        canUseSkill = true;
        skillEffect = transform.FindChild("Effect").gameObject;
    }



    #region ITouchable 멤버

    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
        }
    }

    #endregion


    public void OnSkillMotionEvent()
    {
        ObjectBase[] targets = battleMgr.GetTargets(this, attackRange, 3);
        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].Attacked(this.attackDamage * skillMultiplicationDmg);
        }

        SkillMotionEnd();

    }

    public void OnEffect()
    {
        skillEffect.SetActive(true);
    }
}
