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
        StartCoroutine(SkillProcess());
    }

    IEnumerator SkillProcess()
    {

        ObjectBase[] targets = battleMgr.GetTargets(this, skillRange, 99);
        if (targets == null)
            yield break;

        SkillMotionStart();

        yield return new WaitForSeconds(1f);
        EffectManager.instance.PlayEffect(EffectKind.Doll_SKill, transform.position + new Vector3(3f, -0.3f, 0));
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].Attacked(skillDamage);
        }
    }
}
