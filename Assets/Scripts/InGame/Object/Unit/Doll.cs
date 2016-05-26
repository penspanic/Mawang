using UnityEngine;
using System.Collections;

public class Doll : Warrior
{
    [SerializeField]
    private int skillDamage;
    [SerializeField]
    private float skillRange;

    Marionette owner;

    public void Init(Marionette owner)
    {
        this.owner = owner;
    }

    protected override void Death()
    {
        base.Death();

        if (owner != null)
            owner.OnDollDeath();
    }

    public void OnSkill()
    {
        ObjectBase[] targets = battleMgr.GetTargets(this, skillRange, 99);
        if (targets == null)
            return;

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].Attacked(skillDamage);
        }
    }

    void SetOwnerState()
    {
        //owner.
    }
}
