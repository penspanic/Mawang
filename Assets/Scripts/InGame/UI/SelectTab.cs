﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        msgBox = FindObjectOfType<MessageBox>();
        goldMgr = FindObjectOfType<GoldManager>();
        roseButtonEffectSource = GetComponent<AudioSource>();
        lever = transform.FindChild("Lever");
        unitButtonTrs = transform.FindChild("UnitsButton");
        upgButtonTrs = transform.FindChild("UpgradeButton");
        goldUpgBtn = upgButtonTrs.FindChild("GoldUpgButton").GetComponent<Button>();

        lever.GetComponent<Button>().onClick.AddListener(ClickedRoseButton);

        // Delete this
        if (PlayerData.instance.selectedUnitList.Count == 0)
        {
            PlayerData.instance.selectedUnitList.Add("Skeleton");
            // PlayerData.instance.selectedUnitList.Add("Goblin");
            //PlayerData.instance.selectedUnitList.Add("Orc");
            PlayerData.instance.selectedUnitList.Add("Dullahan");
            PlayerData.instance.selectedUnitList.Add("Grim");
            PlayerData.instance.selectedUnitList.Add("Werewolf");
            PlayerData.instance.selectedUnitList.Add("Witch");
            //sPlayerData.instance.selectedUnitList.Add("Marionette");
            PlayerData.instance.selectedUnitList.Add("Aragog");
        }

        #region Load

        // 선택된 유닛들 스프라이트 로드
        for (int i = 0; i < PlayerData.instance.selectedUnitList.Count; ++i)
            unitPortaitList.Add(SpriteManager.instance.GetSprite(PackingType.UI,
                PlayerData.instance.selectedUnitList[i]));

        // 유닛버튼들 로드
        for (int i = 0; i < 6; ++i)
            unitButtonList.Add(transform.FindChild("UnitsButton").GetChild(i).GetComponent<Image>());

        // 유닛 prefab 추가
        for (int i = 0; i < unitPortaitList.Count; ++i)
        {
            unitPrefabs.Add(Resources.Load<Movable>("Prefabs/OurForce/" +
                PlayerData.instance.selectedUnitList[i]));
        }

        // 유닛버튼에 스프라이트 대입
        for (int i = 0; i < unitButtonList.Count; ++i)
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
        for (int i = 0; i < unitButtonList.Count; ++i)
        {
            int param = i;
            unitButtonList[i].GetComponent<Button>().onClick.AddListener(() => ClikcedUnitButton(param));
        }

        // upgButton Addlistener
        goldUpgBtn.onClick.AddListener(ClickedGoldUpgButton);

        lines = GameObject.FindGameObjectsWithTag("Line");

        #endregion Load
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
        for (int i = 0; i < lines.Length; ++i)
            lines[i].GetComponent<SpriteRenderer>().enabled = set;
    }

    private void ClickedGoldUpgButton()
    {
        if (goldMgr.GetGoldUpgradeStep() >= GoldManager.MaxUpgradeStep)
        {
            msgBox.PushMessage("최대 업그레이드에 도달했습니다.");
        }
        else if (goldMgr.CanGoldUpgrade())
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

    private void ClickedRoseButton()
    {
        if (isMoving)
            return;
        roseButtonEffectSource.Play();

        if (isSelected)
        {
            isSelected = false;
            unitButtonList[prevIdx].color = Color.white;
            LineSetActive(false);
        }

        StartCoroutine(RotateSelectTab());
    }

    public IEnumerator RotateSelectTab()
    {
        float startRot, endRot;
        float unitBtnEndY, upgBtnEndX;
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

        LineSetActive(false);
    }
    
    public Movable GetPrefab(string name)
    {
        return unitPrefabs.Find((eachUnit) =>
        {
            if (eachUnit.name == name)
                return true;
            return false;
        });
    }
}