using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 공주 이미지및 스킬 쿨타임 관리
public class PrincessManager : MonoBehaviour
{
    private BattleManager battleMgr;
    private GameManager gameMgr;
    private PrincessSkillBase currChapterSkill;

    private GameObject princessUI;

    private Image skillName;
    private Image illust;
    private Image portrait;
    private Image portrait_gray;
    private Image castlesprRenderer;

    private List<Movable> ourList = new List<Movable>();

    private int currChapter;
    private float coolTime;
    private float buffDuration;


    private AudioClip princessBGM;

    public Color effectColor
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
        effectColor = Color.white;

        PlayerData.instance.CheckInstance();
        currChapter = PlayerData.instance.GetSelectedChapter();
        currChapterSkill = PrincessSkillBase.CreatePrincessSkill(currChapter);
    }

    private void Start()
    {
        // 쿨타임 가져오기
        StagePattern pattern = JsonManager.instance.GetStagePattern(PlayerData.instance.selectedStage);
        coolTime = pattern.princessCoolTime;
        buffDuration = pattern.buffDuration;

        InitUI();

        SetPrincessBuff();
    }

    // 이미지에 currPrincesse 받은걸로 대입하기
    private void InitUI()
    {
        string chapterName = "C" + currChapter;

        skillName.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, chapterName + "_SkillName");
        illust.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, chapterName + "_L");
        portrait.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, chapterName + "_Portrait");
        castlesprRenderer.sprite = SpriteManager.instance.GetSprite(PackingType.Princess, chapterName + "_CastleImg");

        portrait_gray.sprite = portrait.sprite;

        skillName.SetNativeSize();
        illust.SetNativeSize();


        StartCoroutine(PrincessSkillLoop());
    }

    private void SetPrincessBuff()
    {
        switch (currChapter)
        {
            case 0:
                effectColor = new Color(0.372f, 0.815f, 0.905f, 1);
                break;

            case 1:
                effectColor = new Color(0.76f, 1, 0.73f, 1);
                break;

            case 3:
                effectColor = new Color(0.76f, 1, 0.73f, 1);
                break;
        }
    }

    private IEnumerator PrincessSkillLoop()
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
                StartCoroutine(SkillProcess());
                currTime = 0.0f;
            }
            currTime += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator SkillProcess()
    {
        currChapterSkill.SkillActivate();
        yield return new WaitForSeconds(buffDuration);
        currChapterSkill.SkillInactivate();
    }

    public void PrincessEventSet(bool set)
    {
        princessUI.transform.GetChild(0).gameObject.SetActive(set);
    }
}