using UnityEngine;
using System.Collections;

public class Warrior : Movable
{

    // override된 public 함수가 애니메이션이벤트에 안들어가는 버그
    // --> 그냥 따로 OnAttackEnd 빼서 쓸수밖에없음 
    
    public override void AttackEnd()
    {
        base.AttackEnd();

        ObjectBase[] targets = this.GetTargets();

        if (targets == null)
            return;

        for (int i = 0; i < targets.Length; i++)
            targets[i].Attacked(this.attackDamage);
        canAttack = false;
    }

    public void OnAttackEnd()
    {
        AttackEnd();
    }
}
