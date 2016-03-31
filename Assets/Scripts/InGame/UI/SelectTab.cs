using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectTab : MonoBehaviour
{
    private List<Image> unitButtonList = new List<Image>();
    private List<Sprite> unitPortaitList = new List<Sprite>();
    private List<Movable> unitPrefabs = new List<Movable>();
    private Button goldUpgBtn;

    private MessageBox msgBox;
    private GoldManager goldMgr;

    [SerializeField]
    private float switchTime = 0.5f;
    private bool isMoving = false;
    private int switchFlag = 1;

    private int prevIdx;
    private GameObject[] lines;

    private AudioSource roseButtonEffectSource;

    private Transform lever;
    private Transform unitButtonTrs;
    private Transform upgButtonTrs;

    public bool isSelected
    {
        get;
        set;
    }
    void Awake()
    {
        msgBox = FindObjectOfType<MessageBox>();
        goldMgr = FindObjectOfType<GoldManager>();
        roseButtonEffectSource = GetComponent<AudioSource>();
        lever = transform.FindChild("Lever");
        unitButtonTrs = transform.FindChild("UnitsButton");
        upgButtonTrs = transform.FindChild("UpgradeButton");
        goldUpgBtn = upgButtonTrs.FindChild("GoldUpgButton").GetComponent<Button>();

        lever.GetComponent<Button>().onClick.AddListener(ClickedRoseButton);

        // Temp
        if (PlayerData.instance.playerUnitList.Count == 0)
            PlayerData.instance.playerUnitList.Add("Skeleton");



        #region Load

        // 선택된 유닛들 스프라이트 로드
        for (int i = 0; i < PlayerData.instance.playerUnitList.Count; i++)
            unitPortaitList.Add(SpriteManager.Instance.GetSprite(PackingType.UI,
                PlayerData.instance.playerUnitList[i]));

        // 유닛버튼들 로드
        for (int i = 0; i < 6; i++)
            unitButtonList.Add(transform.FindChild("UnitsButton").GetChild(i).GetComponent<Image>());

        // 유닛 prefab 추가
        for (int i = 0; i < unitPortaitList.Count; i++)
            unitPrefabs.Add(Resources.Load<Movable>("Prefabs/OurForce/" +
                PlayerData.instance.playerUnitList[i]));

        // 유닛버튼에 스프라이트 대입
        for (int i = 0; i < unitButtonList.Count; i++)
        {
            if (i > unitPortaitList.Count - 1)
                unitButtonList[i].GetComponent<Button>().interactable = false;
            else
            {
                unitButtonList[i].sprite = unitPortaitList[i];
                unitButtonList[i].GetComponentInChildren<Text>().text = unitPrefabs[i].GetUnitCost().ToString();
            }
        }

        
        // Button Onclick 추가
        for (int i = 0; i < unitButtonList.Count; i++)
        {
            int param = i;
            unitButtonList[i].GetComponent<Button>().onClick.AddListener(() => ClikcedUnitButton(param));
        }

        // upgButton Addlistener
        goldUpgBtn.onClick.AddListener(ClickedGoldUpgButton);


        lines = GameObject.FindGameObjectsWithTag("Line");
        #endregion


    }



    public void ClikcedUnitButton(int idx)
    {
        // 돈이 부족하면
        if (goldMgr.playerGold < unitPrefabs[idx].GetUnitCost())
            return;


        if (!isSelected) // 유닛이 선택 되지않을시
        {
            isSelected = true;
            unitButtonList[idx].color = Color.gray;
            LineSetActive(true);
            prevIdx = idx;
        }
        else  // 유닛이 선택되있을시
        {
            if (prevIdx == idx) // 같은거 누르면 
            {
                isSelected = false;
                unitButtonList[idx].color = Color.white;
                LineSetActive(false);
            }
            else
            {
                unitButtonList[idx].color = Color.gray;
                unitButtonList[prevIdx].color = Color.white;
                prevIdx = idx;
            }
        }


    }

    public void LineSetActive(bool set)
    {
        for (int i = 0; i < lines.Length; i++)
            lines[i].GetComponent<SpriteRenderer>().enabled = set;
    }

    void ClickedGoldUpgButton()
    {
        if(goldMgr.GetGoldUpgradeStep() >= GoldManager.MaxUpgradeStep)
        {
            msgBox.PushMessage("최대 업그레이드에 도달했습니다.");
        }
        else if(goldMgr.CanGoldUpgrade())
        {
            msgBox.PushMessage("얻는 골드량이 증가합니다!");
            goldMgr.GoldUpgrade();
        }
        else
        {
            msgBox.PushMessage("골드가 부족합니다.");
            return;
        }
    }

    void ClickedRoseButton()
    {

        if (isMoving)
            return;
        roseButtonEffectSource.Play();

        StartCoroutine(RotateSelectTab());
    }


    public IEnumerator RotateSelectTab()
    {
        float startRot, endRot;
        float unitBtnEndY, upgBtnEndX; ;
        isMoving = true;
        float beginTime = Time.unscaledTime;

        switchFlag *= -1;

        if (switchFlag == -1)
        {
            startRot = 0f;
            endRot = -90f;
            upgBtnEndX = 0;
            unitBtnEndY = -700;
        }
        else
        {
            startRot = -90f;
            endRot = 0f;
            upgBtnEndX = -200;
            unitBtnEndY = 0;
        }

        float startX = upgButtonTrs.localPosition.x;
        float startY = unitButtonTrs.localPosition.y;


        // 튜토리얼때도 움직이게하려면 unscaledTime 을 넣는다 
        // -> 그러면 일시정지때도 움직이게됨..
        // -> 따로 일시정지될때를 함수로 뺀다...?
        // TODO: 일시정지 될때도 돌리게 만들기


        while (Time.unscaledTime - beginTime <= switchTime)
        {
            float t = (Time.unscaledTime - beginTime) / switchTime;

            float e = EasingUtil.easeInOutBack(startRot, endRot, t);
            float x = EasingUtil.smoothstep(startX, upgBtnEndX, t);
            float y = EasingUtil.smoothstep(startY, unitBtnEndY, t);

            upgButtonTrs.localPosition = new Vector2(x, upgButtonTrs.localPosition.y);
            unitButtonTrs.localPosition = new Vector2(unitButtonTrs.localPosition.x, y);
            lever.eulerAngles = new Vector3(0, 0, e);

            yield return null;
        }

        // 오차에 대한 조정
        upgButtonTrs.localPosition = new Vector2(upgBtnEndX, upgButtonTrs.localPosition.y);
        unitButtonTrs.localPosition = new Vector2(unitButtonTrs.localPosition.x, unitBtnEndY);
        lever.eulerAngles = new Vector3(0, 0, endRot);

        isMoving = false;
        yield break;
    }

    public Movable GetUnit()
    {
        if (isSelected)
            return unitPrefabs[prevIdx];

        return null;
    }

    public void ResetButton()
    {
        unitButtonList[prevIdx].color = Color.white;
        isSelected = false;
        // prevIdx                         =   99;             // 이게 문제

        LineSetActive(false);
    }
}
