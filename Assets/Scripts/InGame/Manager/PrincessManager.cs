using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum PrincessType
//{
//    chapter0,
//    chapter1,
//    chapter2,
//    chapter3
//}

// 공주 이미지및 스킬 쿨타임 관리 ( 게임매니져로 프로세스 뺄수도있음 )
public class PrincessManager : MonoBehaviour
{
    private BattleManager battleMgr;
    private GameManager gameMgr;

    private float coolTime;
    private float buffDuration;

    private Image skillName;
    private Image illust;
    private Image portrait;
    private Image portrait_gray;
    private Image castlesprRenderer;

    private string currChapter;

    private GameObject princessUI;
    private List<Movable> ourList = new List<Movable>();

    private AudioClip princessBGM;

    public Color buffColor
    {
        get;
        private set;
    }

    // 이미지 위치찾기
    private void Awake()
    {
        battleMgr = GetComponent<BattleManager>();
        gameMgr = GetComponent<GameManager>();
        princessUI = GameObject.Find("PrincessEvent");
        skillName = princessUI.transform.FindChild("Event").FindChild("SkillName").GetComponent<Image>();
        illust = princessUI.transform.FindChild("Event").FindChild("BigIllust").GetComponent<Image>();
        portrait = GameObject.Find("Princess Image").GetComponent<Image>();
        portrait_gray = GameObject.Find("Princess Gray").GetComponent<Image>();
        castlesprRenderer = GameObject.Find("OutpostIcon").GetComponent<Image>();
        buffColor = Color.white;

        PlayerData.instance.CheckInstance();
        currChapter = "C" + PlayerData.instance.GetSelectedChapter().ToString();
    }

    private void Start()
    {
        // 쿨타임 가져오기
        StagePattern pattern = JsonManager.instance.GetStagePattern(PlayerData.instance.selectedStage);
        coolTime = pattern.princessCoolTime;
        buffDuration = pattern.buffDuration;

        InitUI();
    }

    // 이미지에 currPrincesse 받은걸로 대입하기
    private void InitUI()
    {
        skillName.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, currChapter + "_SkillName");
        illust.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, currChapter + "_L");
        portrait.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, currChapter + "_Portrait");
        castlesprRenderer.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, currChapter + "_CastleImg");

        portrait_gray.sprite = portrait.sprite;

        skillName.SetNativeSize();
        illust.SetNativeSize();

        SetPrincessBuff();

        StartCoroutine(PrincessCool());
    }

    private void SetPrincessBuff()
    {
        switch (currChapter)
        {
            case "C0":
                buffColor = new Color(0.372f, 0.815f, 0.905f, 1);
                break;

            case "C1":
                buffColor = new Color(0.76f, 1, 0.73f, 1);
                break;

            case "C3":
                buffColor = new Color(0.76f, 1, 0.73f, 1);
                break;
        }
    }

    private IEnumerator PrincessCool()
    {
        float currTime = 0.0f;
        while (true)
        {
            portrait.fillAmount = currTime / coolTime;
            if (currTime >= coolTime)
            {
                if (!gameMgr.isRun)
                    yield break;
                // 여기서 공주에 따른 효과 발동
                if (PlayerData.instance.selectedStage == "C0S1")
                {
                    if (TutorialManager.instance.oncePrinTuto)
                    {
                        TutorialManager.instance.oncePrinTuto = false;
                        TutorialManager.instance.PlayTutorial(TutorialEvent.PrincessFullGauge);
                        while (Time.timeScale == 0)
                            yield return null;
                    }
                }

                PrincessEventSet(true);
                // ApplyPrincessSkill();
                StartCoroutine(BuffRoutine());
                currTime = 0.0f;
            }
            currTime += Time.deltaTime;

            yield return null;
        }
    }

    private SpriteRenderer[] currSprs;

    private IEnumerator BuffRoutine()
    {
        SetBuff(true);
        yield return new WaitForSeconds(buffDuration);
        SetBuff(false);

        yield break;
    }

    public void SetBuff(bool set)
    {
        ourList.Clear();

        for (int i = 0; i < battleMgr.ourForceList.Count; i++)
            ourList.Add(battleMgr.ourForceList[i].GetComponent<Movable>());

        for (int i = 0; i < ourList.Count; i++)
        {
            ourList[i].isEffecting = set;
            currSprs = ourList[i].GetSprs();

            for (int j = 0; j < currSprs.Length; j++)
            {
                if (currSprs[j].name.Contains("Effect"))
                    continue;

                if (set)
                    currSprs[j].color = buffColor;
                else
                    currSprs[j].color = Color.white;
            }
        }

        switch (currChapter)
        {
            case "C0":
                AttackSpeedDown(ourList, set);
                break;

            case "C1":
                FullHpAll(set);
                break;

            case "C2":
                AttackCastle(set);
                break;

            case "C3":
                HealAllOurForce();
                break;
            case "C4":
                HealandASforEnemy(set);
                break;
            case "C5":
                RandomDieWithLighting(set);
                break;
            case "C6":
                RandomFreezeOurforce(set);
                break;
            case "C7":
                RevivalOurforce(set);
                break;
        }
    }

    #region Skill Impl 

    // C0 Skill
    private void AttackSpeedDown(List<Movable> list, bool set)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (set)
                list[i].AddAttackSpeed(-40);
            else
                list[i].AddAttackSpeed(40);
        }
    }

    // C1 Skill : 아군적군 모두 체력회복
    private void FullHpAll(bool set)
    {
        if (!set)
            return;

        foreach (Movable eachUnit in battleMgr.ourForceList)
            eachUnit.SetFullHP();

        foreach (Movable eachUnit in battleMgr.enemyList)
            eachUnit.SetFullHP();
    }

    // C2 Skill : 우리팀 성 체력달게
    private void AttackCastle(bool set)
    {
        if (!set)
            return;

        battleMgr.ourCastle.GetComponent<SatanCastle>().Attacked(100);
    }

    // C3 Skill : 아군만 체력회복
    private void HealAllOurForce()
    {
        foreach (Movable eachUnit in battleMgr.ourForceList)
        {
            eachUnit.SetFullHP();
        }
    }

    // C4 Skill : 적군 체력 회복 공속 증가
    private void HealandASforEnemy(bool set)
    {

    }
    
    // C5 Skill : 천둥번개 발생 ( 랜덤으로 천둥 뽑아서 사망 ) 
    private void RandomDieWithLighting(bool set)
    {
        if (!set)
            return;
    }
    
    // C6 Skill : 일정확률로 아군 빙결
    private void RandomFreezeOurforce(bool set)
    {
        if (!set)
            return;

    }

    // C7 Skill : 공주 버프가 진행중일때 아군들 계속 부활
    private void RevivalOurforce(bool set)
    {
        // 
    }

    #endregion
    public void PrincessEventSet(bool set)
    {
        princessUI.transform.GetChild(0).gameObject.SetActive(set);
    }
}