using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderLayerManager : MonoBehaviour
{
    private BattleManager battleMgr;
    private List<ObjectBase> allList = new List<ObjectBase>();
    private List<ObjectBase> lineListArr = new List<ObjectBase>();
    private int orderInterval = 12;

    private Queue<int> deathOrderQueue = new Queue<int>();

    private void Awake()
    {
        battleMgr = GetComponent<BattleManager>();
    }

    /// <summary>
    /// 오더 정렬하는 함수 파라미터에 라인 넘버 넣어주면됨.
    /// 호출 할시기 : 유닛이 생성될때
    /// </summary>
    /// <param name="lineNum"></param>
    public void UpdateOrder(int lineNum)
    {
        lineListArr.Clear();
        allList.Clear();

        allList.AddRange(battleMgr.enemyList);
        allList.AddRange(battleMgr.ourForceList);

        lineListArr = battleMgr.GetSameLine(allList, lineNum);

        lineListArr.Sort((a, b) =>
        {
            float A = a.transform.position.y + a.GetComponent<Movable>().GetAdjustPos().y;
            float B = b.transform.position.y + a.GetComponent<Movable>().GetAdjustPos().y;

            if (A < B)
                return 1;
            else if (A > B)
                return -1;
            else
                return 0;
        });

        for (int i = 0; i < lineListArr.Count; ++i)
        {
            SpriteRenderer[] sprs = lineListArr[i].GetComponent<Movable>().GetSprs();
            for (int j = 0; j < sprs.Length; ++j)
            {
                sprs[j].sortingOrder += i * orderInterval;
            }
        }
    }
}