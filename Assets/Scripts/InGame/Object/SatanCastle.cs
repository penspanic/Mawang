using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SatanCastle : Castle, ITouchable
{
    [SerializeField] private GameObject skillObject;
    private float skillCoolTime; // 스킬 쿨타임
    private int skillDamage;
    private Image      castlePortrait;

    protected override void Awake()
    {
        base.Awake();
        castlePortrait  =   GameObject.Find("Castle Image").GetComponent<Image>();
        
        PlayerData.instance.CheckInstance();
        skillCoolTime = CastleUpgrade.GetUpgradeApplyedValue("Cool Time");
        skillDamage = CastleUpgrade.GetUpgradeApplyedValue("Damage");
        maxHP = CastleUpgrade.GetUpgradeApplyedValue("Hp");
        hp = maxHP;

        
    }
    void Start()
    {
        StartCoroutine(SkillProcess());
    }

    private void UseSkill()
    {
        if (!canUseSkill)
            return;

        // 스킬구현
        ObjectBase[] targets = GetTargets();

        if (targets.Length == 0)
            return;

        canUseSkill = false;

       


        SatanSkill skill;
        for (int i = 0; i < targets.Length; i++)
        {
            // 스킬 구현
            
            skill = (Instantiate(skillObject, targets[i].transform.position, new Quaternion()) as GameObject).
                GetComponent<SatanSkill>();
            skill.attackDamage = skillDamage;
            skill.SetTarget(targets[i]);
        }


        StopAllCoroutines();
        StartCoroutine(SkillProcess());
    }


    IEnumerator SkillProcess()
    {
        while (!canUseSkill)
        {

            if (skillElapsedTime >= skillCoolTime)
            {
                castlePortrait.fillAmount = 1;
                skillElapsedTime = 0;
                canUseSkill = true;
            }
            else
            {
                castlePortrait.fillAmount = skillElapsedTime / skillCoolTime;
                skillElapsedTime += Time.deltaTime * Time.timeScale;
            }
            yield return null;
        }

    }

    // Use Only in FixItem.cs
    public void UseFixItem(float hpHealRate)
    {
        float healValue = maxHP * hpHealRate;

        if(healValue + hp > maxHP) // 아이템을 사용했을 때 최대로 회복될 때
        {
            hp = maxHP;
        }
        else
        {
            hp += (int)healValue;
        }
    }
    public override ObjectBase[] GetTargets()
    {
        List<ObjectBase> oppositeList = battleMgr.GetOpposite(true);
        oppositeList = battleMgr.SelectInRange(oppositeList, transform.position, this.attackRange);
        return oppositeList.ToArray();
    }

    public void OnTouch()
    {   
        UseSkill();
    }
}
