using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bandsman : Movable
{
    [SerializeField]
    private float buffDuration;
    [SerializeField]
    private int growthAttack;
    [SerializeField]
    private Sprite buff_AS;


    private List<ObjectBase>    lineList = new List<ObjectBase>();
    private GameObject          skillEffect;


    protected override void Attack()
    {
        Debug.Log(attackInterval);
        

        animator.speed = 1;
        animator.Play("Attack", 0);
    }
    public override void AttackEnd()
    {
        Debug.Log("attackEnd");
        canAttack = false;
        StartCoroutine(BuffRoutine());
    }

    IEnumerator BuffRoutine()
    {
        lineList = battleMgr.enemyList.FindAll(e => e.line == line &&
            this.attackRange * battleMgr.fightDistance > Mathf.Abs(transform.position.x - e.transform.position.x));

        lineList.Remove(this);
        Debug.Log(lineList.Count);
        BuffSet(true);
        yield return new WaitForSeconds(buffDuration);
        BuffSet(false);
    }

    void BuffSet(bool set)
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            if(lineList[i] == null)
                continue;

            if (set)
                lineList[i].GetComponent<ObjectBase>().SetAddAttackDmg(growthAttack);
            else
                lineList[i].GetComponent<ObjectBase>().SetAddAttackDmg(-growthAttack);
        }
    }

    public void OnAttackEnd()
    {
        AttackEnd();

    }
}
