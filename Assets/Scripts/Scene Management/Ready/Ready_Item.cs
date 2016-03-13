using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Ready_Item : MonoBehaviour
{
    public Button[] itemButtons;
    public Text[] itemAmountTexts;
    public Button buyButton;
    public Text itemDescriptionText;
    public Text obsidianText;

    int selectedIndex;

    int[] itemsCost;
    string[] itemsDescription;

    void Awake()
    {
        JsonManager.instance.CheckInstance();
        PlayerData.instance.CheckInstance();
        obsidianText.text = PlayerData.instance.obsidian.ToString();

        itemsCost = new int[itemButtons.Length];
        itemsDescription = new string[itemButtons.Length];
        for (int i = 0; i < itemButtons.Length; i++)
        {
            string name = itemButtons[i].name;
            itemsCost[i] = JsonManager.instance.GetCost(name);
            itemsDescription[i] = JsonManager.instance.GetDescription(name);
            itemAmountTexts[i].text = PlayerData.instance.itemStorage[name].ToString();
        }
    }

    void SetBuyButtonState(bool value)
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
    }

    public void OnItemBuyButtonDown()
    {
        string name = itemButtons[selectedIndex].name;
        PlayerData.instance.PurchaseItem(name, itemsCost[selectedIndex]);
        obsidianText.text = PlayerData.instance.obsidian.ToString();
        itemAmountTexts[selectedIndex].text = PlayerData.instance.itemStorage[name].ToString();
        if (PlayerData.instance.obsidian - itemsCost[selectedIndex] < 0)
            SetBuyButtonState(false);
    }
}
