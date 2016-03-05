using UnityEngine;
using System.Collections;

public class Shielder : Movable
{
    private Vector3 prevPos = Vector3.zero;

    protected override void Attack()
    {


        if(prevPos == transform.position)
            return;

        prevPos     =   transform.position;
        PlaySound(attackSound);
        animator.speed = 1;
        animator.Play("Attack", 0);

    }
    public override void AttackEnd()
    {

    }

    public void OnAttackEnd()
    {
        AttackEnd();

    }
}
