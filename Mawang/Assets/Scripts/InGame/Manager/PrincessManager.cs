using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


// 공주 이미지및 스킬 쿨타임 관리 ( 게임매니져로 프로세스 뺄수도있음 ) 
public class PrincessManager : MonoBehaviour
{
    //public string currPrincss
    //{
    //    get;
    //    set;
    //}
    private BattleManager   battleMgr;

    [SerializeField]
    private float       coolTime;

    [SerializeField]
    private float       buffDuration;

    
    private Image       skillName;
    private Image       illust;
    private Image       portrait;
    private Image       portrait_gray;
    private Image       castlesprRenderer;

    private string currChapter;

    private GameObject  princessUI;
    private List<Movable> ourList   =   new List<Movable>();

    private AudioClip   princessBGM;

    public Color buffColor
    {
        get;
        private set;
    }

    // 이미지 위치찾기
    void Awake()
    {
        battleMgr       =   GetComponent<BattleManager>();
        princessUI      =   GameObject.Find("PrincessEvent");
        skillName       =   princessUI.transform.FindChild("Event").FindChild("SkillName").GetComponent<Image>();
        illust          =   princessUI.transform.FindChild("Event").FindChild("BigIllust").GetComponent<Image>();
        portrait        =   GameObject.Find("Princess Image").GetComponent<Image>();
        portrait_gray   =   GameObject.Find("Princess Gray").GetComponent<Image>();
        castlesprRenderer       =   GameObject.Find("OutpostIcon").GetComponent<Image>();


        PlayerData.instance.CheckInstance();
        currChapter = "C" + PlayerData.instance.GetSelectedChapter().ToString();

        InitUI();

    }
    // 이미지에 currPrincesse 받은걸로 대입하기
    void InitUI()
    {
        skillName.sprite        =   Resources.Load<Sprite>("Sprite/Princess/" + currChapter + "_SkillName");
        illust.sprite           =   Resources.Load<Sprite>("Sprite/Princess/" + currChapter + "_L");
        portrait.sprite         =   Resources.Load<Sprite>("Sprite/Princess/" + currChapter + "_Portait");
        castlesprRenderer.sprite        =   Resources.Load<Sprite>("Sprite/Princess/" + currChapter + "_CastlesprRenderer");

        portrait_gray.sprite    =   portrait.sprite;

        SetPrincessBuff();

        StartCoroutine(PrincessCool());
        
    }

    void SetPrincessBuff()
    {
        switch(currChapter)
        {
            case "C0":
                buffColor   =   new Color(0.372f, 0.815f, 0.905f, 1);
                break;
        }
    }

    IEnumerator PrincessCool()
    {
        float currTime = 0.0f;
        while (true)
        {
            portrait.fillAmount = currTime / coolTime;
            if (currTime >= coolTime)
            {
                PrincessEventSet(true);
                // 여기서 공주에 따른 효과 발동
                StartCoroutine(BuffRoutine());
                currTime = 0.0f;
            }
            currTime += Time.deltaTime;

            yield return null;
        }
    }

    SpriteRenderer[] currSprs;

    IEnumerator BuffRoutine()
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
            ourList[i].isEffecting  =   set;
            currSprs                =   ourList[i].GetSprs();

            for (int j = 0; j < currSprs.Length; j++)
            {
                if(set)
                    currSprs[j].color   =   buffColor;
                else
                    currSprs[j].color   =   Color.white;
            }
        }

        switch (currChapter)
        {
            case "C0":
                AttackSpeedDown(ourList,set);
                break;
        }
    }


    void AttackSpeedDown(List<Movable> list, bool set)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(set)
                list[i].AddAttackSpeed(-40);
            else
                list[i].AddAttackSpeed(40);
        }
    }

    public void PrincessEventSet(bool set)
    {
        princessUI.transform.GetChild(0).gameObject.SetActive(set);
    }


}
