using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ready_UnitSelect : MonoBehaviour
{
    public ListView unitView;

    public GameObject unitSelectButton;
    public Text unitDescription;
    public Text selectedCountText;

    List<string> unitNameList;
    Button[] unitButtons;
    Image[] checkedImages;
    Image[] selectedImages;
    bool[] unitSelected;

    int selectedCount = 0;


    void Awake()
    {
        PlayerData.instance.CheckInstance();
        JsonManager.instance.CheckInstance();

        unitNameList = PlayerData.instance.playerUnitList;

        SetUnitList();
        selectedCountText.text = "0/" + unitNameList.Count.ToString();
    }

    void SetUnitList()
    {
        Sprite portrait;
        List<Button> unitButtonList = new List<Button>();
        List<Image> checkedImageList = new List<Image>();
        List<Image> selectedImageList = new List<Image>();

        unitSelected = new bool[unitNameList.Count];
        unitView.column = 3;
        unitView.row = unitNameList.Count / 3 + ((unitNameList.Count % 3) == 0 ? 0 : 1);
        unitView.itemCount = unitNameList.Count;
        unitView.SetItems();


        Button newButton;
        Image checkedImage;
        Image selectedImage;
        for (int i = 0; i < unitNameList.Count; i++)
        {
            portrait = SpriteManager.Instance.GetSprite(PackingType.UI, unitNameList[i]);
            newButton = unitView.GetItem(i).GetComponent<Button>();
            newButton.image.sprite = portrait;

            int param = i;
            newButton.onClick.AddListener(() => OnUnitButtonDown(param));
            unitButtonList.Add(newButton);

            checkedImage = newButton.transform.FindChild("Check").GetComponent<Image>();
            checkedImageList.Add(checkedImage);
            selectedImage = newButton.transform.FindChild("Select").GetComponent<Image>();
            selectedImageList.Add(selectedImage);
        }
        unitButtons = unitButtonList.ToArray();
        checkedImages = checkedImageList.ToArray();
        selectedImages = selectedImageList.ToArray();
    }

    int selectedUnitIndex;
    public void OnUnitButtonDown(int index) // 현재 유닛 설정
    {
        checkedImages[selectedUnitIndex].enabled = false;

        selectedUnitIndex = index;
        checkedImages[selectedUnitIndex].enabled = true;
        unitDescription.text = JsonManager.instance.GetKoreanName(unitNameList[index]) + "\n\n" +
            JsonManager.instance.GetDescription(unitNameList[index]);

        unitSelectButton.GetComponentInChildren<Text>().text = unitSelected[selectedUnitIndex] ? "취소" : "선택";
    }

    public void OnSelectButtonDown()
    {
        if (selectedCount == 6 && !unitSelected[selectedUnitIndex])
            return;

        checkedImages[selectedUnitIndex].enabled = true;
        unitSelected[selectedUnitIndex] = !unitSelected[selectedUnitIndex];
        selectedImages[selectedUnitIndex].enabled = unitSelected[selectedUnitIndex];
        unitSelectButton.GetComponentInChildren<Text>().text = unitSelected[selectedUnitIndex] ? "취소" : "선택";

        selectedCount = 0;
        for(int i = 0;i<unitSelected.Length;i++)
        {
            if (unitSelected[i])
                selectedCount++;
        }

        selectedCountText.text = selectedCount.ToString() + "/" + unitNameList.Count.ToString();
    }
}
