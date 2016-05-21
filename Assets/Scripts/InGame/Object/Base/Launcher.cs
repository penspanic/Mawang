using UnityEngine;

public class Launcher : Movable
{
    [SerializeField]
    private GameObject projectile;

    protected ObjectBase[] targets;

    protected override void Attack()
    {
        base.Attack();

        targets = this.GetTargets();

        if (targets == null)
            return;
    }

    public void OnProjectile()
    {
        for (int i = 0; i < targets.Length; i++)   // canHitNum이 1인 우선 전제하에
        {
            projectile.SetActive(true); // OnEnable, OnDisalbe 쓸꺼(바꿀수도있음)
        }
    }

    private void OnAttackEnd()
    {
        AttackEnd();
    }

    public ObjectBase GetLauncherTargets(int idx)
    {
        if (targets[idx] == null)
            return null;

        return targets[idx];
    }

    public override void AttackEnd()
    {
        base.AttackEnd();
        canAttack = false;
    }
}