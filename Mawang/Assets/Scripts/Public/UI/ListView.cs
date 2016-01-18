using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum XAlignment
{
    Left,
    Middle,
    Right,
}
public enum YAlignment
{
    Top,
    Middle,
    Bottom,
}
/// <summary>
/// 행렬로 아이템을 정렬해서 추가해 주는 UI이다.
/// </summary>
public class ListView : MonoBehaviour
{
    public GameObject content;
    public GameObject item;

    public int row;
    public int column;
    public int itemCount;

    public int xInterval;
    public int yInterval;

    [SerializeField]
    XAlignment xAlignment;
    [SerializeField]
    YAlignment yAlignment;
    private List<GameObject[]> itemList = new List<GameObject[]>();
    private object[][] items;
    private Vector2 itemSize;

    bool isLeftToRight = true;  // 아이템을 왼쪽에서 오른쪽으로 배치할 지
    bool isTopToButtom = true;  // 아이템을 위에서 아래로 배치할 지
    void Awake()
    {

    }

    public void SetItems()
    {
        RectTransform contentTransform = content.GetComponent<RectTransform>();
        Vector2 firstItemPos = new Vector2();
        itemSize = item.GetComponent<RectTransform>().sizeDelta;
        // Content 크기 조정
        int contentWidth = (int)contentTransform.sizeDelta.x;
        int contentHeight = (int)contentTransform.sizeDelta.y;

        if (itemSize.x * column + xInterval * (column + 1) > contentWidth)
            contentWidth = (int)itemSize.x * column + yInterval * (column + 1);

        if (itemSize.y * row + yInterval * (row + 1) > contentHeight)
            contentHeight = (int)itemSize.y * row + yInterval * (row + 1);

        contentTransform.sizeDelta = new Vector2(contentWidth, contentHeight);

        // 첫번째 item 위치 설정, 간격 재설정
        Vector2 leftTop = new Vector2(-contentTransform.rect.size.x / 2, contentTransform.rect.size.y / 2);

        switch (xAlignment)
        {
            case XAlignment.Left:
                firstItemPos.x = leftTop.x + xInterval + itemSize.x / 2;
                break;
            case XAlignment.Middle:
                float xLength = itemSize.x * column + xInterval * (column - 1);
                firstItemPos.x = -(xLength / 2) + itemSize.x / 2;
                break;
            case XAlignment.Right:
                isLeftToRight = false;
                float contentRight = leftTop.x + contentTransform.sizeDelta.x;
                firstItemPos.x = contentRight - (xInterval + itemSize.x / 2);
                break;
        }
        switch (yAlignment)
        {
            case YAlignment.Top:
                firstItemPos.y = leftTop.y - (itemSize.y / 2 + yInterval);
                break;
            case YAlignment.Middle:
                float yLength = itemSize.y * row + yInterval * (row - 1);
                firstItemPos.y = -(-(yLength / 2) + itemSize.y / 2);
                break;
            case YAlignment.Bottom:
                isTopToButtom = false;
                float contentBottom = leftTop.y - contentTransform.sizeDelta.y;
                firstItemPos.y = contentBottom + (yInterval + itemSize.y / 2);
                break;
        }


        GameObject newItem;
        for (int y = 0; y < row; y++)
        {
            List<GameObject> columnList = new List<GameObject>();
            for (int x = 0; x < column; x++)
            {
                if (y * column + x >= itemCount)
                    break;
                newItem = Instantiate(item);
                newItem.transform.SetParent(content.transform, false);
                newItem.transform.localPosition = new Vector2(
                    firstItemPos.x + (xInterval + itemSize.x) * x * (isLeftToRight == true ? 1 : -1),
                    firstItemPos.y - (yInterval + itemSize.y) * y * (isTopToButtom == true ? 1 : -1));
                columnList.Add(newItem);
            }
            itemList.Add(columnList.ToArray());
        }

        // Content 초기 위치 설정
        content.transform.localPosition = new Vector2(content.transform.localPosition.x, -(contentTransform.sizeDelta.y / 2));
    }

    public GameObject GetItem(int x, int y)
    {
        return itemList[y][x];
    }
    public GameObject GetItem(int index)
    {
        return itemList[index / column][index % column];
    }
}