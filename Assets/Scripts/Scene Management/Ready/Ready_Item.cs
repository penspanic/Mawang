using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Ready_Item : MonoBehaviour
{
    public ListView itemView;

    Button[] itemButtons;
    Image[] checkImages;
    Text[] itemAmountTexts;
    string[] itemsName;

    public Button buyButton;
    public Text itemDescriptionText;
    public Text obsidianText;

    private Ready ready;

    private int selectedIndex = 0;

    private int[] itemsCost;
    private string[] itemsDescription;
    private List<int> selectedIndexList = new List<int>();
    private List<string> itemNameList = new List<string>();

    private void Awake()
    {
        JsonManager.instance.CheckInstance();
        PlayerData.instance.CheckInstance();
        ready = GameObject.FindObjectOfType<Ready>();

        SetItemList();
        SetSelectedItem();
        SetBuyButtonState(false);
    }

    private void SetItemList()
    {
        Sprite itemSprite;

        int itemCount = PlayerData.instance.itemStorage.Count;

        itemButtons = new Button[itemCount];
        checkImages = new Image[itemCount];
        itemAmountTexts = new Text[itemCount];
        itemsName = new string[itemCount];

        itemsCost = new int[itemCount];
        itemsDescription = new string[itemCount];

        int i = 0;


        itemView.column = 3;
        itemView.row = itemCount / 3 + ((itemCount % 3) == 0 ? 0 : 1);
        itemView.itemCount = itemCount;
        itemView.SetItems();

        foreach(string eachName in PlayerData.instance.itemStorage.Keys)
        {
            itemNameList.Add(eachName);
            itemsName[i] = eachName;
            itemSprite = SpriteManager.instance.GetSprite(PackingType.UI, eachName);

            itemsCost[i] = JsonManager.instance.GetCost(eachName);
            itemsDescription[i] = JsonManager.instance.GetDescription(eachName);

            
            itemButtons[i] = itemView.GetItem(i).GetComponent<Button>();
            itemButtons[i].GetComponent<Image>().sprite = itemSprite;
            itemAmountTexts[i] = itemButtons[i].transform.FindChild("Text").GetComponent<Text>();
            checkImages[i] = itemButtons[i].transform.FindChild("Check").GetComponent<Image>();
            itemAmountTexts[i].text = PlayerData.instance.itemStorage[eachName].ToString();

            int param = i;
            itemButtons[i].onClick.AddListener(() => OnItemButtonDown(param));

            ++i;
        }
    }

    private void SetSelectedItem()
    {
        string[] selectedItems = PlayerData.instance.selectedItemList.ToArray();

        for(int i = 0;i<selectedItems.Length;++i)
        {
            selectedIndexList.Add(itemNameList.IndexOf(selectedItems[i]));
        }
        SetCheckImages();
    }

    private void SetBuyButtonState(bool value)
    {
        buyButton.interactable = value;
    }

    public void OnItemButtonDown(int index)
    {
        selectedIndex = index;
        itemDescriptionText.text = "가격 : " + itemsCost[index].ToString() + "\n\n" + itemsDescription[index];

        if (PlayerData.instance.obsidian - itemsCost[index] >= 0)
            SetBuyButtonState(true);
        else
            SetBuyButtonState(false);

        if (selectedIndexList.Contains(index)) // 이미 선택되었을 경우
        {
            selectedIndexList.Remove(index);
            checkImages[index].enabled = false;
        }
        else
        {
            if (selectedIndexList.Count < 3)
                selectedIndexList.Add(index);
        }

        SetCheckImages();
    }

    private void SetCheckImages()
    {
        PlayerData.instance.selectedItemList.Clear();

        for(int i = 0;i<itemView.itemCount;++i)
        {
            if (selectedIndexList.Contains(i))
            {
                checkImages[i].enabled = true;
                PlayerData.instance.selectedItemList.Add(itemNameList[i]);
            }
            else
                checkImages[i].enabled = false;
        }
    }

    public void OnItemBuyButtonDown()
    {
        string name = itemsName[selectedIndex];
        PlayerData.instance.PurchaseItem(name, itemsCost[selectedIndex]);
        ready.ResetObsidianText();
        itemAmountTexts[selectedIndex].text = PlayerData.instance.itemStorage[name].ToString();
        if (PlayerData.instance.obsidian - itemsCost[selectedIndex] < 0)
            SetBuyButtonState(false);
    }
}