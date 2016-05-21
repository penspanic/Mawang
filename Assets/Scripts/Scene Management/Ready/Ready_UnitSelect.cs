using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ready_UnitSelect : MonoBehaviour
{
    public ListView unitView;

    public GameObject unitSelectButton;
    public Text unitDescription;
    public Text selectedCountText;

    private List<string> unitNameList;
    private Button[] unitButtons;
    private Image[] checkedImages;
    private Image[] selectedImages;
    private bool[] unitSelected;

    private int selectedCount = 0;
    private int selectedUnitIndex = 0;

    private void Awake()
    {
        PlayerData.instance.CheckInstance();
        JsonManager.instance.CheckInstance();

        unitNameList = PlayerData.instance.playerUnitList;

        SetUnitList();
        SetSelectedUnit();
        selectedCountText.text = selectedCount.ToString() + "/6";
    }

    private void SetUnitList()
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
            portrait = SpriteManager.instance.GetSprite(PackingType.UI, unitNameList[i]);
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

    private void SetSelectedUnit()
    {
        if (PlayerData.instance.selectedUnitList.Count != 0)
        {
            foreach (string eachName in PlayerData.instance.selectedUnitList)
            {
                selectedUnitIndex = unitNameList.IndexOf(eachName);
                OnSelectButtonDown();
            }
        }
    }

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
        for (int i = 0; i < unitSelected.Length; i++)
        {
            if (unitSelected[i])
                selectedCount++;
        }

        selectedCountText.text = selectedCount.ToString() + "/" + "6";
    }

    public string[] GetSelectedUnit()
    {
        List<string> unitsList = new List<string>();
        int i = 0;
        foreach (string eachName in unitNameList)
        {
            if (unitSelected[i])
                unitsList.Add(eachName);
            i++;
        }
        return unitsList.ToArray();
    }
}