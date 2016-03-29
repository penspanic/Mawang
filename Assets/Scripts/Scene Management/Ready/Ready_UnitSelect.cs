using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ready_UnitSelect : MonoBehaviour
{
    Ready ready;
    public ListView unitView;
    public Sprite checkedSprite;

    //public GameObject unitMaxPopUp;
    public GameObject unitSelectButton;
    public Text unitDescription;
    bool[] isUnitChecked;
    readonly Vector2 unitButtonStartPos = new Vector2(50, -50);
    readonly int unitButtonXInterval = 90;
    readonly int unitButtonYInterval = -90;

    List<string> unitNameList = new List<string>();
    Button[] unitButtons;
    Image[] checkedImages;

    NotifyBar notifyBar;

    void Awake()
    {
        ready = GameObject.FindObjectOfType<Ready>();
        PlayerData.instance.CheckInstance();
        JsonManager.instance.CheckInstance();

        unitNameList = PlayerData.instance.playerUnitList;

        SetUnitList();

        notifyBar = GameObject.FindObjectOfType<NotifyBar>();
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
            portrait = Resources.Load<Sprite>("Sprite/UI/Icon/UnitPortrait/" + unitNameList[i]);
            newButton = unitView.GetItem(i).GetComponent<Button>();
            newButton.image.sprite = portrait;

            int param = i;
            newButton.onClick.AddListener(() => OnUnitButtonDown(param));
            unitButtonList.Add(newButton);

            Image checkedImage = new GameObject().AddComponent<Image>();
            checkedImage.sprite = checkedSprite;
            checkedImage.enabled = false;
            checkedImage.transform.SetParent(newButton.transform, false);
            checkedImageList.Add(checkedImage);
        }
        unitButtons = unitButtonList.ToArray();
        checkedImages = checkedImageList.ToArray();
    }

    int selectedUnitIndex;
    public void OnUnitButtonDown(int index) // 현재 유닛 설정
    {
        selectedUnitIndex = index;
        unitDescription.text = JsonManager.instance.GetType(unitNameList[index]) + "\n\n" +
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
