using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    private BattleManager   battleMgr;

    private Sprite      blueMark;
    private Sprite      redMark;
    private GameObject  markPrefab;

    [SerializeField]
    private float   markIntervalScale   =   7.5f;
    [SerializeField]
    private float   markAllPos   =   50;

    List<Vector2>   ourForceDrawPos     = new List<Vector2>();
    List<Vector2>   enemyDrawPos        = new List<Vector2>();

    List<Image>     ourForceMarkPool    = new List<Image>();
    List<Image>     enemyMarkPool       = new List<Image>();

    void Awake()
    {
        battleMgr   = FindObjectOfType<BattleManager>();

        blueMark    = SpriteManager.Instance.GetSprite(PackingType.UI,"MiniMap_blue");
        redMark     = SpriteManager.Instance.GetSprite(PackingType.UI, "MiniMap_red");

        markPrefab  = Resources.Load<GameObject>("Prefabs/UI/Mark");
        
    }

    void Start()
    {
        StartCoroutine(MiniMapProcess());
    }

    IEnumerator MiniMapProcess()
    {
        while (true)
        {
            FindDrawPos();
            DrawMark();
            yield return null;
        }
    }

    #region FindPos

    void FindDrawPos()
    {
        ourForceDrawPos.Clear();
        enemyDrawPos.Clear();

        FindUnitListPos(ourForceDrawPos,battleMgr.ourForceList);
        FindUnitListPos(enemyDrawPos,battleMgr.enemyList);
    }

    void FindUnitListPos(List<Vector2> miniMapPosList, List<ObjectBase> objList)
    {
        Vector2 drawPos;
        for (int i = 0; i < objList.Count; i++)
        {
                drawPos     =   GetMiniMapPos(objList[i].transform.position,
                    objList[i].line);
                miniMapPosList.Add(drawPos);
        }
    }

    Vector2 GetMiniMapPos(Vector2 pos,int line)
    {
        pos.x = (pos.x * markIntervalScale) - markAllPos;
        pos.y = 12;
        pos.y = (line - 2) * -12;

        return pos;
    }

    #endregion

    #region Draw

    void DrawMark()
    {
        for (int i = 0; i < ourForceMarkPool.Count; i++)
            ourForceMarkPool[i].gameObject.SetActive(false);
        for (int i = 0; i < enemyMarkPool.Count; i++)
            enemyMarkPool[i].gameObject.SetActive(false);


        DrawUnit(ourForceDrawPos,ourForceMarkPool,true);
        DrawUnit(enemyDrawPos, enemyMarkPool, false);
    }

    void DrawUnit(List<Vector2> drawPosList, List<Image> markPool,bool isBlue)
    {
        for (int i = 0; i < drawPosList.Count; i++)
        {
            if (markPool.Count < drawPosList.Count)
                CreateMark(isBlue);

            MarkSpawn(markPool[i], i, isBlue);
        }
    }

    void CreateMark(bool isBlue)
    {
        GameObject newMark = Instantiate(markPrefab);
        newMark.transform.SetParent(transform, false);

        if (isBlue)
        {
            newMark.GetComponent<Image>().sprite = blueMark;
            ourForceMarkPool.Add(newMark.GetComponent<Image>());
        }
        else
        {
            newMark.GetComponent<Image>().sprite = redMark;
            enemyMarkPool.Add(newMark.GetComponent<Image>());
        }
    }

    void MarkSpawn(Image pool,int idx,bool isBlue)
    {
        pool.gameObject.SetActive(true);

        if (isBlue)
            pool.transform.localPosition = ourForceDrawPos[idx];
        else
            pool.transform.localPosition = enemyDrawPos[idx];
    }

    #endregion
}
