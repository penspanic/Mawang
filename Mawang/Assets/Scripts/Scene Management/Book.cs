using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public struct UnitInfo
{
    public Sprite portrait;
    public Movable unit;
    
}
public class Book : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public ListView bookView;
    List<UnitInfo> unitsData = new List<UnitInfo>();

    public GameObject pageMarkPrefab;

    public int pageIntervalX;
    public int pageSizeX;
    public int pageSizeY;

    int currPageIndex; 
    void Awake()
    {
        PlayerData.instance.CheckInstance();
        JsonManager.instance.CheckInstance();

        // Sprite/UI/Icon/Unitortrait 경로 안에 있는 스프라이트 갯수만큼 생성됨 
        Sprite[] portraits = Resources.LoadAll<Sprite>("Sprite/UI/Icon/UnitPortrait");
        Movable currUnit;
        for (int i = 0; i < portraits.Length; i++)
        {
            string name = portraits[i].name;
            currUnit = Resources.Load<Movable>("Prefabs/OurForce/" + name);
            if (currUnit == null)
                continue;
            unitsData.Add(new UnitInfo() { portrait = portraits[i], unit = currUnit });
        }

        SetPages();
        SetPageMark();

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeIn(1f));
        yield return new WaitForSeconds(1f);
        isChanging = false;
    }

    int[] xPositions; // Page들의 위치
    Text[] descriptions;
    void SetPages()
    {
        bookView.row = 1;
        bookView.column = unitsData.Count;
        bookView.xInterval = 100;
        bookView.itemCount = unitsData.Count;
        bookView.SetItems();

        content.sizeDelta = new Vector2(bookView.itemCount * (pageSizeX + bookView.xInterval), 0);
        content.anchoredPosition = new Vector2(content.sizeDelta.x / 2 - bookView.xInterval / 2 - pageSizeX / 2, 0);

        List<Text> descriptionList = new List<Text>();
        xPositions = new int[bookView.itemCount];
        Movable currUnit;
        for (int i = 0; i < bookView.itemCount; i++)
        {
            xPositions[i] = (int)(content.anchoredPosition.x - i * (pageSizeX + bookView.xInterval));
            descriptionList.Add(bookView.GetItem(i).GetComponentInChildren<Text>());

            // Set Description
            currUnit = unitsData[i].unit;
            string unitType = currUnit is Launcher ? "원거리" : "근거리";
            descriptionList[i].text = string.Format("{0}\n\n생산 비용 : {1}\n체력 : {2}\n공격력 : {3}\n{4}\n{5}\n{6}",
                currUnit.name, currUnit.GetUnitCost(), currUnit.GetHP(), currUnit.GetAttackDamage(), unitType, "스킬 정보", JsonManager.instance.GetJoke(currUnit.name));

            Movable newUnit = Instantiate<Movable>(currUnit);
            newUnit.transform.SetParent(bookView.GetItem(i).transform);
            newUnit.transform.localPosition = new Vector2(-300, 0);
            newUnit.transform.Translate(newUnit.GetAdjustPos());
            newUnit.transform.localScale *= 1.5f;
            newUnit.SetSortingLayer("UI Over");
        }
        descriptions = descriptionList.ToArray();
    }

    public int markLeftX;
    public int markRightX;
    Image[] marks;
    void SetPageMark()
    {
        Debug.Log(markLeftX - markRightX);
        Debug.Log(bookView.itemCount);
        int xInterval = Mathf.Abs(markLeftX - markRightX) / bookView.itemCount;
        GameObject markParent = GameObject.Find("Page Marks");
        List<Image> markList = new List<Image>();
        for (int i = 0; i < bookView.itemCount; i++)
        {
            markList.Add(Instantiate(pageMarkPrefab).GetComponent<Image>());
            markList[i].transform.SetParent(markParent.transform, false);
            markList[i].color = Color.black;
            markList[i].transform.localPosition = new Vector2(markLeftX + xInterval * i + xInterval / 2, 0);
        }
        marks = markList.ToArray();
        marks[0].color = Color.red;
    }

    void ChangeMarkColor(int index)
    {
        marks[currPageIndex].color = Color.black;
        currPageIndex = index;
        marks[currPageIndex].color = Color.red;
    }

    void Update()
    {
        if (isMovePage)
        {
            // 37, 0
            content.transform.Translate(new Vector2(targetX - content.anchoredPosition.x, 0) * Time.deltaTime / 15, Space.Self);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isChanging)
                EscapeProcess();
        }
    }

    bool isMovePage = false;
    int targetX;

    // Connected to Scoll View -> Event Trigger -> Begin Drag
    public void OnDragBegin()
    {
        isMovePage = false;
    }

    // Connected to Scoll View -> Event Trigger -> End Drag
    public void  OnDragEnd()
    {
        // 1650, 550, -550, -1650
        // 차가 1100
        Debug.Log(content.anchoredPosition);

        float x = Mathf.Abs(content.anchoredPosition.x);
        int mul = (int)x / (int)(pageSizeX + pageIntervalX);

        float distanceX = float.MaxValue;
        int targetIndex = 0;
        for (int i = 0; i < xPositions.Length; i++)
        {
            if (Mathf.Abs(xPositions[i] - content.anchoredPosition.x) < distanceX)
            {
                distanceX = Mathf.Abs(xPositions[i] - content.anchoredPosition.x);
                targetX = xPositions[i];
                targetIndex = i;
            }
        }
        ChangeMarkColor(targetIndex);
        Debug.Log(targetX);
        isMovePage = true;
    }

    bool isChanging = false;
    void EscapeProcess()
    {
        isChanging = true;
        StartCoroutine(SceneFader.Instance.FadeOut(1f, "Main"));
        ButtonSound.PlaySound(ButtonSound.SoundType.BackSound);
    }
}