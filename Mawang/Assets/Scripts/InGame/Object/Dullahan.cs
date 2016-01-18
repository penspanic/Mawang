using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Dullahan : Warrior, ITouchable
{
    [SerializeField]
    private float   skillDistance;
    private GameObject skillEffect;


    protected override void Awake()
    {
        base.Awake();
        skillEffect     =   transform.FindChild("Effect").gameObject;
        canUseSkill     =   true;
    }

    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            skillEffect.SetActive(true);
            StartCoroutine(DullahanSkill());
        }
    }


    List<ObjectBase> lineList   =   new List<ObjectBase>();
    IEnumerator DullahanSkill()
    {
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
                    lineList[i].transform.Translate(skillDistance * Time.deltaTime * Time.timeScale, 0, 0);
            }
            yield return null;
        }
        yield break;
    }


    // Animation Event
    public void OnSkillMotionEvent()
    {
        skillEffect.SetActive(false);
        SkillMotionEnd();
    }

}
