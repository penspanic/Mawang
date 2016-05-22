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

    private void Awake()
    {
        JsonManager.instance.CheckInstance();
        PlayerData.instance.CheckInstance();
        ready = GameObject.FindObjectOfType<Ready>();

        SetItemList();
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

            i++;
        }
        
    }

    private void SetBuyButtonState(bool value)
    {
        buyButton.interactable = value;
    }

    public void OnItemButtonDown(int index)
    {
        checkImages[selectedIndex].enabled = false;
        selectedIndex = index;
        checkImages[selectedIndex].enabled = true;
        itemDescriptionText.text = "가격 : " + itemsCost[index].ToString() + "\n\n" + itemsDescription[index];

        if (PlayerData.instance.obsidian - itemsCost[index] >= 0)
            SetBuyButtonState(true);
        else
            SetBuyButtonState(false);
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