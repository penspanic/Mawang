using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Book : MonoBehaviour
{
    public GameObject pagePrefab;
    public Image leftButtonImage;
    public Image rightButtonImage;

    private List<GameObject> pageList = new List<GameObject>();
    private AudioSource bookEffectSource;

    private int selectedIndex = 0;

    private void Awake()
    {
        bookEffectSource = GetComponent<AudioSource>();
        SetBook();
        ShowPage(0, false);

        this.gameObject.SetActive(false);
    }

    private void SetBook()
    {
        // Sprite/UI/Icon/Unitortrait 경로 안에 있는 스프라이트 갯수만큼 생성됨
        List<Movable> unitList = new List<Movable>(Resources.LoadAll<Movable>("Prefabs/OurForce"));
        unitList.Sort();

        Text nameText;
        Text descriptionText;
        Movable eachUnit = null;

        for(int i = 0;i<unitList.Count;++i)
        {
            eachUnit = unitList[i];

            GameObject currPage = Instantiate(pagePrefab);
            currPage.transform.SetParent(this.transform, false);
            currPage.transform.SetAsFirstSibling();

            nameText = currPage.transform.Find("Name Text").GetComponent<Text>();
            descriptionText = currPage.transform.Find("Description Text").GetComponent<Text>();
            nameText.text = JsonManager.instance.GetKoreanName(eachUnit.name);
            string unitType = "타입 : " + (eachUnit is Launcher ? "원거리" : "근거리");
            descriptionText.text = string.Format("생산 비용 : {0}\n체력 : {1}\n공격력 : {2}\n{3}\n\n{4}\n\n{5}",
                eachUnit.GetUnitCost(), eachUnit.GetHP(), eachUnit.GetAttackDamage(), unitType, "스킬 : " +
                JsonManager.instance.GetDescription(eachUnit.name), JsonManager.instance.GetJoke(eachUnit.name));
            // Show Unit

            Movable newUnit = Instantiate(eachUnit);
            newUnit.transform.SetParent(currPage.transform);
            newUnit.transform.localPosition = new Vector2(-250, 0);
            newUnit.transform.Translate(newUnit.GetAdjustPos());
            newUnit.transform.localScale *= 1.5f;
            newUnit.SetSortingLayer("UI Over");

            // Color Set
            if (!PlayerData.instance.playerUnitList.Contains(eachUnit.name)) // 캐릭터를 보유하고 있지 않을 때
            {
                SpriteRenderer[] sprs = newUnit.GetSprs();
                for (int j = 0; j < sprs.Length; ++j)
                    sprs[j].color = Color.black;
            }

            pageList.Add(currPage);
        }
    }

    private void ShowPage(int index, bool soundPlay)
    {
        if (soundPlay)
            bookEffectSource.Play();

        GameObject eachPage = null;
        for(int i = 0;i<pageList.Count;++i)
        {
            eachPage = pageList[i];

            eachPage.SetActive(false);
            if (i == index)
                eachPage.SetActive(true);
        }

        if (selectedIndex == 0)
        {
            leftButtonImage.color = new Color(1, 1, 1, 0.5f);
        }
        else if (selectedIndex == pageList.Count - 1)
        {
            rightButtonImage.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            leftButtonImage.color = new Color(1, 1, 1, 1);
            rightButtonImage.color = new Color(1, 1, 1, 1);
        }
    }

    public void OnLeftButtonDown()
    {
        if (selectedIndex == 0)
            return;
        ShowPage(--selectedIndex, true);
    }

    public void OnRightButtonDown()
    {
        if (selectedIndex == pageList.Count - 1)
            return;
        ShowPage(++selectedIndex, true);
    }

    public void OnCloseButtonDown()
    {
        this.gameObject.SetActive(false);
    }
}