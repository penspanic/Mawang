using UnityEngine;

public class Warrior : Movable
{
    [SerializeField]
    private bool isFirstJugmentAttack;

    // override된 public 함수가 애니메이션이벤트에 안들어가는 버그
    // --> 그냥 따로 OnAttackEnd 빼서 쓸수밖에없음

    public override void AttackEnd()
    {
        base.AttackEnd();

        if (!isFirstJugmentAttack)
        {
            ObjectBase[] targets = this.GetTargets();

            if (targets == null)
                return;

            for (int i = 0; i < targets.Length; ++i)
                targets[i].Attacked(this.attackDamage);
        }

        canAttack = false;
    }

    public void OnAttackEnd()
    {
        AttackEnd();
    }

    // 공격판정 이벤트
    public void OnJudgmentAttack()
    {
        JudgmentAttack();
    }

    protected virtual void JudgmentAttack()
    {
        ObjectBase[] targets = this.GetTargets();

        if (targets == null)
            return;

        for (int i = 0; i < targets.Length; ++i)
            targets[i].Attacked(this.attackDamage);
    }
}