using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    public Transform[] buttonTransforms;


    void Awake()
    {
        SetItemButton();
    }

    void SetItemButton()
    {
        for (int i = 0; i < buttonTransforms.Length; i++)
        {
            if (i >= PlayerData.instance.selectedItemList.Count)
                break;
            string itemName = PlayerData.instance.selectedItemList[i];

            GameObject buttonPrefab = Resources.Load<GameObject>("Prefabs/UI/InGame Item/" + itemName);

            GameObject newButton = Instantiate<GameObject>(buttonPrefab);
            newButton.transform.SetParent(buttonTransforms[i]);
            newButton.transform.localScale = Vector3.one;
            newButton.transform.localPosition = Vector3.one;
        }
    }
}