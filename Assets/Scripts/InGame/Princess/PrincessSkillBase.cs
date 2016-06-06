using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PrincessSkillBase : MonoBehaviour
{
    protected BattleManager battleMgr;
    protected Color effectColor;

    protected virtual void Awake()
    {
        battleMgr = GameObject.FindObjectOfType<BattleManager>();
    }

    protected virtual void Update()
    {

    }

    public void SkillActivate()
    {
        StartCoroutine(SkillStart());
    }

    public void SkillInactivate()
    {
        StartCoroutine(SkillEnd());
    }

    protected void SetEffectColor(bool set, ICollection<Movable> targets)
    {
        SpriteRenderer[] currSprs = null;
        foreach(Movable eachUnit in targets)
        {
            if (eachUnit == null)
                continue;

            eachUnit.isEffecting = set;
            currSprs = eachUnit.GetSprs();

            for(int i = 0;i<currSprs.Length;++i)
            {
                if (set)
                    currSprs[i].color = effectColor;
                else
                    currSprs[i].color = Color.white;
            }
        }
    }

    public static PrincessSkillBase CreatePrincessSkill(int chapter)
    {
        GameObject skillObject = new GameObject(chapter + "Princess Skill");
        switch(chapter)
        {
            case 0 :
                skillObject.AddComponent<C0PrincessSkill>();
                break;
            case 1:
                skillObject.AddComponent<C1PrincessSkill>();
                break;
            case 2:
                skillObject.AddComponent<C2PrincessSkill>();
                break;
            case 3:
                skillObject.AddComponent<C3PrincessSkill>();
                break;
            case 4:
                skillObject.AddComponent<C4PrincessSkill>();
                break;
            case 5:
                skillObject.AddComponent<C5PrincessSkill>();
                break;
            case 6:
                skillObject.AddComponent<C6PrincessSkill>();
                break;
            case 7:
                skillObject.AddComponent<C7PrincessSkill>();
                break;
            default:
                throw new UnityException(string.Format("Not Implemented chapter{0} princess skill!", chapter));
        }
        return skillObject.GetComponent<PrincessSkillBase>();
    }

    protected abstract IEnumerator SkillStart();

    protected abstract IEnumerator SkillEnd();
}