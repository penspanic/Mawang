using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dullahan : Warrior, ITouchable
{
    [SerializeField]
    private float   skillDistance;


    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            Vector2 spawnPos = transform.position;
            spawnPos += new Vector2(-0.57f,-1.2f);
            EffectManager.instance.PlayEffect(EffectKind.Dulahan_skill,spawnPos,transform);
            StartCoroutine(DullahanSkill());
        }
    }


    List<ObjectBase> lineList   =   new List<ObjectBase>();
    List<ObjectBase> dmgUnitList   =   new List<ObjectBase>();
    IEnumerator DullahanSkill()
    {
        dmgUnitList.Clear();
        while (isSkillMotion)
        {
             lineList   =   battleMgr.enemyList.FindAll(e => e.line == line);

            if (transform.position.x >= 17.4f)
            {
                transform.position  =   new Vector2(17.4f, transform.position.y);
                yield break;
            }


            transform.Translate(skillDistance * Time.deltaTime * Time.timeScale, 0, 0);


            for (int i = 0; i < lineList.Count; i++)
            {
                if (this.attackRange > Mathf.Abs(transform.position.x - lineList[i].transform.position.x))
                {
                    // dmgUnit 에 유닛이 없을경우
                    if(!dmgUnitList.Contains(lineList[i]))
                    {
                        lineList[i].Attacked(attackDamage);
                        dmgUnitList.Add(lineList[i]);
                    }
                    lineList[i].transform.Translate(skillDistance * Time.deltaTime * Time.timeScale, 0, 0);
                }
            }
            yield return null;
        }
        yield break;
    }


    // Animation Event
    public void OnSkillMotionEvent()
    {
        SkillMotionEnd();
    }

}
