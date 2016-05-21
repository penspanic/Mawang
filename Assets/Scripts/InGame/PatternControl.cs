using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternControl : MonoBehaviour
{
    private List<Transform> enemyList = new List<Transform>();

    private Vector3 prevRandomPos;
    private Vector3 randomAdjustPos;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
            enemyList.Add(transform.GetChild(i));

        StartCoroutine(EnmeyLiveCheck());
    }

    private IEnumerator EnmeyLiveCheck()
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