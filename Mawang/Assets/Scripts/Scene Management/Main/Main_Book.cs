﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Main_Book : MonoBehaviour
{
    public GameObject pagePrefab;
    public Image leftButtonImage;
    public Image rightButtonImage;

    List<GameObject> pageList = new List<GameObject>();
    AudioSource bookEffectSource;

    int selectedIndex = 0;

    void Awake()
    {
        bookEffectSource = GetComponent<AudioSource>();
        SetBook();
        ShowPage(0);

        this.gameObject.SetActive(false);
    }

    void SetBook()
    {
        // Sprite/UI/Icon/Unitortrait 경로 안에 있는 스프라이트 갯수만큼 생성됨 
        List<Movable> unitList = new List<Movable>(Resources.LoadAll<Movable>("Prefabs/OurForce"));
        unitList.Sort();

        Text nameText;
        Text descriptionText;
        foreach (Movable eachUnit in unitList)
        {

            GameObject currPage = Instantiate(pagePrefab);
            currPage.transform.SetParent(this.transform, false);
            currPage.transform.SetAsFirstSibling();

            nameText = currPage.transform.Find("Name Text").GetComponent<Text>();
            descriptionText = currPage.transform.Find("Description Text").GetComponent<Text>();
            nameText.text = eachUnit.name;
            string unitType = eachUnit is Launcher ? "원거리" : "근거리";
            descriptionText.text = string.Format("{0}\n\n생산 비용 : {1}\n체력 : {2}\n공격력 : {3}\n{4}\n{5}\n{6}",
                eachUnit.name, eachUnit.GetUnitCost(), eachUnit.GetHP(), eachUnit.GetAttackDamage(), unitType, "스킬 정보", JsonManager.instance.GetJoke(eachUnit.name));
            // Show Unit

            Movable newUnit = Instantiate(eachUnit);
            newUnit.transform.SetParent(currPage.transform);
            newUnit.transform.localPosition = new Vector2(-250, 0);
            newUnit.transform.Translate(newUnit.GetAdjustPos());
            newUnit.transform.localScale *= 1.5f;
            newUnit.SetSortingLayer("UI Over");

            pageList.Add(currPage);
        }
    }

    void ShowPage(int index)
    {
        bookEffectSource.Play();
        int i = 0;
        foreach (GameObject eachPage in pageList)
        {
            eachPage.SetActive(false);
            if (index == i)
                eachPage.SetActive(true);
            i++;
        }

        if(selectedIndex == 0)
        {
            leftButtonImage.color = new Color(1, 1, 1, 0.5f);
        }
        else if(selectedIndex == pageList.Count - 1)
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
        ShowPage(--selectedIndex);
    }

    public void OnRightButtonDown()
    {
        if (selectedIndex == pageList.Count - 1)
            return;
        ShowPage(++selectedIndex);
    }

    public void OnCloseButtonDown()
    {
        selectedIndex = 0;
        this.gameObject.SetActive(false);
    }
}