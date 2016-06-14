using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<ObjectBase> ourForceList = new List<ObjectBase>();
    public List<ObjectBase> enemyList = new List<ObjectBase>();

    public GameObject ourCastle { get; private set; }
    public GameObject enemyCastle { get; private set; }

    public static float tileSize
    {
        get
        {
            return 1.4f;
        }
    }

    private void Awake()
    {
        ourCastle = GameObject.Find("SatanCastle");
        enemyCastle = GameObject.Find("Outpost");
    }

    public void AddObject(ObjectBase obj)
    {
        if (obj.isOurForce)
            ourForceList.Add(obj);
        else
            enemyList.Add(obj);
    }

    public void RemoveObject(ObjectBase obj)
    {
        if (obj.isOurForce)
            ourForceList.Remove(obj);
        else
            enemyList.Remove(obj);
    }

    public ObjectBase[] GetTargets(ObjectBase obj, float attackRange, int canHitNum, bool isFindOur = false)
    {
        List<ObjectBase> oppositeList = new List<ObjectBase>();

        if (isFindOur)
            oppositeList = GetOpposite(!obj.isOurForce);
        else
            oppositeList = GetOpposite(obj.isOurForce);

        oppositeList = GetSameLine(oppositeList, obj.line);
        oppositeList = SelectInRange(oppositeList, obj.transform.position.x, attackRange, isFindOur);

        if (oppositeList.Count == 0)    // 없을때는 성공격하거나 null
        {
            if (CanAttackCastle(oppositeList, obj))
                return oppositeList.ToArray();
            return null;
        }

        List<ObjectBase> returnList = new List<ObjectBase>();

        for (int i = 0; i < oppositeList.Count; ++i)
        {
            if (i < canHitNum)
                returnList.Add(oppositeList[i]);
        }
        return returnList.ToArray();
    }

    public List<ObjectBase> GetOpposite(bool isOur) // 맞은편 반환
    {
        if (isOur)
            return enemyList;
        else
            return ourForceList;
    }

    public List<ObjectBase> GetSameLine(List<ObjectBase> list, int line) // 같은 라인 반환
    {
        list = list.FindAll(eachObj => (eachObj.line == line && eachObj.isDestroyed == false));

        return list;
    }

    public List<ObjectBase> SelectInRange(List<ObjectBase> list, float xPivot, float range, bool findSameForce = false) // 거리되는애들 리턴
    {
        List<ObjectBase> returnList = new List<ObjectBase>();

        ObjectBase target = null;

        for (int i = 0; i < list.Count; ++i)
        {
            target = list[i];

            if (IsInRange(target, xPivot, range, findSameForce))
                returnList.Add(target);
        }

        returnList.Sort((a, b) =>
        {
            float A = Mathf.Abs(xPivot - a.transform.position.x);
            float B = Mathf.Abs(xPivot - b.transform.position.x);

            if (A > B)
                return 1;
            else if (A < B)
                return -1;
            else
                return 0;
        });

        return returnList;
    }

    bool IsInRange(ObjectBase target, float xPivot, float range, bool findSameForce)
    {
        float realRange = range * tileSize;

        float distX = 0;
        if (!target.isDestroyed)
        {
            if (target.isOurForce)
                distX = xPivot - target.transform.position.x;
            else
                distX = target.transform.position.x - xPivot;

            if (findSameForce)
            {
                if (target.isOurForce)
                    distX = target.transform.position.x - xPivot;
                else
                    distX = xPivot - target.transform.position.x;
            }

            if (0 < distX && distX < realRange)
                return true;
        }
        return false;
    }

    public Movable[] GetAllUnitInLine(int line)
    {
        List<Movable> returnList = new List<Movable>();

        InsertSameLineMovable(ourForceList, returnList, line);
        InsertSameLineMovable(enemyList, returnList, line);

        return returnList.ToArray();
    }

    void InsertSameLineMovable(List<ObjectBase> srcList, List<Movable> destList, int line)
    {
        for (int i = 0; i < srcList.Count; ++i)
        {
            if (SameLineAndMovable(srcList[i], line))
                destList.Add(srcList[i] as Movable);
        }
    }

    bool SameLineAndMovable(ObjectBase obj, int line)
    {
        return obj is Movable && obj.line == line;
    }

    public bool CanAttackCastle(List<ObjectBase> list, ObjectBase obj)   // 성이 공격 가능한 범위인지 판단하고 리스트에 추가
    {
        switch (obj.line)
        {
            case 1:
                if (obj.GetAttackRange() >= Mathf.Abs(obj.transform.position.x
                    - IsOppositeCastle(obj).transform.FindChild("Spawn Line1").position.x))
                {
                    list.Add(IsOppositeCastle(obj).GetComponent<ObjectBase>());
                    return true;
                }
                break;

            case 2:
                if (obj.GetAttackRange() >= Mathf.Abs(obj.transform.position.x -
                    IsOppositeCastle(obj).transform.FindChild("Spawn Line2").position.x))
                {
                    list.Add(IsOppositeCastle(obj).GetComponent<ObjectBase>());
                    return true;
                }
                break;

            case 3:
                if (obj.GetAttackRange() >= Mathf.Abs(obj.transform.position.x -
                    IsOppositeCastle(obj).transform.FindChild("Spawn Line3").position.x))
                {
                    list.Add(IsOppositeCastle(obj).GetComponent<ObjectBase>());
                    return true;
                }
                break;
        }
        return false;
    }

    public GameObject IsOppositeCastle(ObjectBase obj)
    {
        if (obj.isOurForce)
            return enemyCastle;
        else
            return ourCastle;
    }
}