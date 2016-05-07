using UnityEngine;
using System.Collections;


/// <summary>
/// 2. Movable 에다가 OnTouch 두고 if문 만들어서
/// 스킬구현만 여기로 빼기.
/// 
/// </summary>
public class Goblin : Launcher, ITouchable
{
    [SerializeField]
    private GameObject  skillProjectile;
    [SerializeField]
    private int       addDamage;


    // SkillAnimation Event
    public void OnSkillProjectile()
    {
        attackDamage    =   attackDamage + addDamage;
        skillProjectile.SetActive(true);
        attackDamage    =   attackDamage - addDamage;


        SkillMotionEnd();
    }

    public void OnTouch()
    {
        if (state != MovableState.Advance && canUseSkill && !isDestroyed)
            SkillMotionStart();
    }
}
