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
    private GameObject buff_AS;

    bool once = true;


    private List<ObjectBase>    lineList = new List<ObjectBase>();
    private GameObject          skillEffect;


    protected override void Attack()
    {
        animator.speed = 1;
        animator.Play("Attack", 0);

        if (once)
        {
            Vector2 spawnPos = transform.position;
            spawnPos += new Vector2(-1.2f, 0.4f);
            EffectManager.Instance.PlayEffect(EffectKind.Bandsman_skill, spawnPos);
            once = false;
        }

    }
    public override void AttackEnd()
    {
        canAttack = false;
        once = true;
        StartCoroutine(BuffRoutine());
    }

    IEnumerator BuffRoutine()
    {
        lineList = battleMgr.enemyList.FindAll(e => e.line == line &&
            this.attackRange * battleMgr.fightDistance > Mathf.Abs(transform.position.x - e.transform.position.x));

        lineList.Remove(this);

        AddBuffsprRenderer();
        BuffSet(true);
        yield return new WaitForSeconds(buffDuration);
        BuffSet(false);
    }

    void AddBuffsprRenderer()
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            if (lineList[i].transform.FindChild("Bandsman_buff(Clone)") != null)
            {
                GameObject tmp = lineList[i].transform.FindChild("Bandsman_buff(Clone)").gameObject;
                tmp.GetComponent<SpriteRenderer>().color = Color.white;
                tmp.SetActive(true);
                continue;
            }


            GameObject go = Instantiate(buff_AS);
            go.SetActive(true);

            go.transform.SetParent(lineList[i].transform);

            go.transform.localPosition = new Vector2(0, 2.6f);
            AdjustBuffPos(go, lineList[i].name);
        }
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


    public void AdjustBuffPos(GameObject go, string name)
    {
        switch (name)
        {
            //case "Pawn(Clone)":
            //    go.transform.localPosition += new Vector3(0f, 0.4f);
            //    break;
            //case "Orc(Clone)":
            //    go.transform.localPosition += new Vector3(0.25f, 1.4f);
            //    break;
            //case "Grim(Clone)":
            //    go.transform.localPosition += new Vector3(0.6f, -0.2f);
            //    break;
            //case "Dullahan(Clone)":
            //    go.transform.localPosition += new Vector3(-0.5f, -0.6f);
            //    break;
            //case "Harpy(Clone)":
            //    go.transform.localPosition += new Vector3(1.1f, 0.5f);
            //    break;
        }
    }
    public void OnAttackEnd()
    {
        AttackEnd();

    }
}
