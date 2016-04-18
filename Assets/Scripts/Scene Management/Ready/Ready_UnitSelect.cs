﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ready_UnitSelect : MonoBehaviour
{
    public ListView unitView;

    //public GameObject unitMaxPopUp;
    public GameObject unitSelectButton;
    public Text unitDescription;
    bool[] isUnitChecked;

    List<string> unitNameList = new List<string>();
    Button[] unitButtons;
    Image[] checkedImages;

    void Awake()
    {
        PlayerData.instance.CheckInstance();
        JsonManager.instance.CheckInstance();

        unitNameList = PlayerData.instance.playerUnitList;

        SetUnitList();
    }

    void SetUnitList()
    {
        Sprite portrait;
        List<Button> unitButtonList = new List<Button>();
        List<Image> checkedImageList = new List<Image>();
        isUnitChecked = new bool[unitNameList.Count];
        unitView.column = 3;
        unitView.row = unitNameList.Count / 3 + ((unitNameList.Count % 3) == 0 ? 0 : 1);
        unitView.itemCount = unitNameList.Count;
        unitView.SetItems();

        Button newButton;
        for (int i = 0; i < unitNameList.Count; i++)
        {
            portrait = SpriteManager.Instance.GetSprite(PackingType.UI, unitNameList[i]);
            newButton = unitView.GetItem(i).GetComponent<Button>();
            newButton.image.sprite = portrait;

            int param = i;
            newButton.onClick.AddListener(() => OnUnitButtonDown(param));
            unitButtonList.Add(newButton);

            Image checkedImage = newButton.transform.FindChild("Check").GetComponent<Image>();
            checkedImageList.Add(checkedImage);
        }
        unitButtons = unitButtonList.ToArray();
        checkedImages = checkedImageList.ToArray();
    }

    int selectedUnitIndex;
    public void OnUnitButtonDown(int index) // 현재 유닛 설정
    {
        checkedImages[selectedUnitIndex].enabled = false;

        selectedUnitIndex = index;
        checkedImages[selectedUnitIndex].enabled = true;
        unitDescription.text = JsonManager.instance.GetKoreanName(unitNameList[index]) + "\n\n" +
            JsonManager.instance.GetDescription(unitNameList[index]);

        //SetSelectButtonText();

    }
    //void SetSelectButtonText()
    //{
    //    if (isUnitChecked[selectedUnitIndex])
    //    {
    //        unitSelectButton.GetComponentInChildren<Text>().text = "취소";
    //    }
    //    else
    //    {
    //        unitSelectButton.GetComponentInChildren<Text>().text = "선택";
    //    }
    //}

    //public void OnUnitSelectButonDown()
    //{
    //    // 7개 초과인지 체크
    //    int selectedNum = 0;
    //    foreach (bool eachValue in isUnitChecked)
    //    {
    //        if (eachValue == true)
    //            selectedNum++;
    //    }
    //    if (selectedNum == 6 && isUnitChecked[selectedUnitIndex] == false)
    //    {
    //        notifyBar.ShowMessage("6종류만 선택할 수 있습니다!");
    //    }
    //    else
    //    {
    //        isUnitChecked[selectedUnitIndex] = !isUnitChecked[selectedUnitIndex];
    //        if (isUnitChecked[selectedUnitIndex])
    //        {
    //            checkedImages[selectedUnitIndex].enabled = true;
    //        }
    //        else
    //        {
    //            checkedImages[selectedUnitIndex].enabled = false;
    //        }
    //    }
    //    SetSelectButtonText();
    //}
    //public bool OnGameStart()
    //{
    //    PlayerData.instance.selectedUnitList.Clear();

    //    for (int i = 0; i < unitButtons.Length; i++)
    //    {
    //        if (isUnitChecked[i])
    //        {
    //            PlayerData.instance.selectedUnitList.Add(unitNameList[i]);
    //        }
    //    }

    //    if (PlayerData.instance.selectedUnitList.Count == 0)
    //        return false;

    //    return true;
    //}
}
