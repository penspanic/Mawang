using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternControl : MonoBehaviour 
{
    List<Transform>     enemyList = new List<Transform>();

    private Vector3 prevRandomPos;
    private Vector3 randomAdjustPos;

    void Awake()
    {
        for(int i = 0;i<transform.childCount;i++)
            enemyList.Add(transform.GetChild(i));

        StartCoroutine(EnmeyLiveCheck());
    }

    IEnumerator EnmeyLiveCheck()
    {
        while (enemyList.Count != 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i] == null)
                    enemyList.RemoveAt(i);
            }
            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}
