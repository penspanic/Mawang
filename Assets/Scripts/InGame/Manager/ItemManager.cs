using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    public Transform[] buttonTransforms;


    void Awake()
    {
        SetItem(PlayerData.instance.selectedItemList.ToArray());
    }

    public void SetItem(string[] itemNames)
    {
        if (itemNames.Length > 3)
            throw new System.ArgumentException("아이템 갯수는 3개까지만 가능합니다!");

        for (int i = 0; i < itemNames.Length; ++i)
        {
            GameObject buttonPrefab = Resources.Load<GameObject>("Prefabs/UI/InGame Item/" + itemNames[i]);

            GameObject newButton = Instantiate<GameObject>(buttonPrefab);
            newButton.transform.SetParent(buttonTransforms[i]);
            newButton.transform.localScale = Vector3.one;
            newButton.transform.localPosition = Vector3.one;
        }
    }
}