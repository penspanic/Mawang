using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Ready_EnemyInfo : MonoBehaviour
{

    public ListView enemyView;
    Ready ready;
    GameObject[] enemysData;
    Image[] enemyImage;
    Text[] enemyDescription;

    void Awake()
    {
        ready = GameObject.FindObjectOfType<Ready>();
        PlayerData.instance.CheckInstance();
        if (PlayerData.instance.selectedStage == null)
            PlayerData.instance.selectedStage = "C0S1";
        SetEnemyList();
    }

    void SetEnemyList()
    {
        string[] enemyName = JsonManager.instance.GetAppearEnemyName(PlayerData.instance.selectedStage);

        enemyView.column = 1;
        enemyView.row = enemyName.Length;
        enemyView.itemCount = enemyName.Length;
        enemyView.SetItems();

        List<GameObject> dataList = new List<GameObject>();
        List<Image> imageList = new List<Image>();
        List<Text> textList = new List<Text>();
        for (int i = 0; i < enemyView.itemCount; i++)
        {
            dataList.Add(enemyView.GetItem(i));
            imageList.Add(dataList[i].GetComponentInChildren<Image>());
            textList.Add(dataList[i].GetComponentInChildren<Text>());
        }
        enemysData = dataList.ToArray();
        enemyImage = imageList.ToArray();
        enemyDescription = textList.ToArray();

        for (int i = 0; i < enemysData.Length; i++)
        {

            enemysData[i].transform.localPosition = new Vector2(
                enemysData[i].transform.localPosition.x - 120, enemysData[i].transform.localPosition.y);
            enemyImage[i].sprite = SpriteManager.instance.GetSprite(PackingType.UI, enemyName[i]);
            enemyDescription[i].text = JsonManager.instance.GetDescription(enemyName[i]);

        }
    }
}
