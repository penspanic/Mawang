using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SatanCastle : Castle
{
    [SerializeField]
    private GameObject skillObject;
    [SerializeField]
    Button skillButton;
    private float skillCoolTime; // 스킬 쿨타임
    private int skillDamage;
    private int skillCnt;

    private Image castlePortrait;
    private Image castleWhite;

    protected override void Awake()
    {
        base.Awake();
        PlayerData.instance.CheckInstance();

        castlePortrait = GameObject.Find("Castle Image").GetComponent<Image>();
        castleWhite = GameObject.Find("Castle White").GetComponent<Image>();
        Image castleGrayImage = GameObject.Find("Castle Gray").GetComponent<Image>();
        //Sprite castleIconSprite = SpriteManager.Instance.GetSprite(PackingType.UI,"Castle_Lv" + CastleInfo.GetCastleLevel().ToString());
        Sprite castleIconSprite = Resources.Load<Sprite>("Sprite/UI/InGame/castle_lv" + CastleInfo.GetCastleLevel().ToString());

        Image starImage = GameObject.Find("Star").GetComponent<Image>();
        starImage.sprite = SpriteManager.Instance.GetSprite(PackingType.UI, "Star_" + CastleInfo.GetCastleLevel().ToString());

        castlePortrait.sprite = castleIconSprite;
        castleGrayImage.sprite = castleIconSprite;

        skillCoolTime = CastleUpgrade.GetUpgradeApplyedValue("Cool Time");
        skillDamage = CastleUpgrade.GetUpgradeApplyedValue("Damage");
        maxHP = CastleUpgrade.GetUpgradeApplyedValue("Hp");

        hp = maxHP;

        skillCnt = 40;

        if (PlayerData.instance.selectedStage == "C0S1")
            skillCoolTime = 35;


        skillButton.onClick.AddListener(OnTouch);


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
        for (int i = 0; i < skillCnt; i++)
        {
            // 스킬 구현
            if (i == targets.Length)
                break;

            skill = (Instantiate(skillObject, targets[i].transform.position, new Quaternion()) as GameObject).
                GetComponent<SatanSkill>();
            skill.attackDamage = skillDamage;
            skill.SetTarget(targets[i]);
        }


        StopCoroutine(SkillProcess());
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
                CheckTuto();
                canUseSkill = true;
            }
            else
            {
                castlePortrait.fillAmount = skillElapsedTime / skillCoolTime;
                skillElapsedTime += Time.deltaTime * Time.timeScale;
            }
            yield return null;
        }
        StartCoroutine(SkillTwinkle());
    }

    IEnumerator SkillTwinkle()
    {
        float elapsedTime = 0f;
        while (canUseSkill)
        {
            elapsedTime += Time.deltaTime;
            castleWhite.color = new Color(1, 1, 1, (Mathf.Sin((elapsedTime - 1.57f) * 5f) * 0.5f + 0.5f) * 0.3f);
            yield return null;
        }
        castleWhite.color = new Color(1, 1, 1, 0);
    }

    void CheckTuto()
    {
        if (PlayerData.instance.selectedStage == "C0S1")
        {
            if (TutorialManager.Instance.onceCastleTuto)
            {
                TutorialManager.Instance.PlayTutorial(TutorialEvent.CastleFullGauge);
                TutorialManager.Instance.onceCastleTuto = false;
            }
        }
    }

    // Use Only in FixItem.cs
    public void UseFixItem(float hpHealRate)
    {
        float healValue = maxHP * hpHealRate;

        if (healValue + hp > maxHP) // 아이템을 사용했을 때 최대로 회복될 때
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