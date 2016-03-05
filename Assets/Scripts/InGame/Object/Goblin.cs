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
    private int       amplificationDamage;


    protected override void Awake()
    {
        base.Awake();
        if (forDecoration)
            return;
        canUseSkill     =   true;
    }

    // SkillAnimation Event
    public void OnSkillProjectile()
    {
        attackDamage    =   attackDamage * amplificationDamage;
        skillProjectile.SetActive(true);
        attackDamage    =   attackDamage / amplificationDamage;


        SkillMotionEnd();
    }

    public void OnTouch()
    {
        if (state != MovableState.Advance && canUseSkill && !isDestroyed)
            SkillMotionStart();
    }
}
