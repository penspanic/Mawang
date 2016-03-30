using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteOrderLayerManager : MonoBehaviour
{
    BattleManager battleMgr;
    List<ObjectBase> allList = new List<ObjectBase>();
    List<ObjectBase> lineListArr = new List<ObjectBase>();
    int orderInterval   =   6;
    
    Queue<int> deathOrderQueue = new Queue<int>();

    void Awake()
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

        lineListArr = battleMgr.GetSameLine(allList,lineNum);

        lineListArr.Sort((a, b) =>
        {
            float A = a.transform.position.y;
            float B = b.transform.position.y;

            if (A < B)
                return 1;
            else if (A > B)
                return -1;
            else
                return 0;
        });


        for (int i = 0; i < lineListArr.Count; i++)
        {
            SpriteRenderer[] sprs =  lineListArr[i].GetComponent<Movable>().GetSprs();
            for (int j = 0; j < sprs.Length; j++)
            {
                sprs[j].sortingOrder += i * orderInterval;
            }
        }

    }


}
