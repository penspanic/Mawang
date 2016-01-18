using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternControl : MonoBehaviour 
{
    List<Transform>     enemyList = new List<Transform>();

    [SerializeField]
    private float randomY_min = -0.1f;
    [SerializeField]
    private float randomY_max = 0.1f;


    private Vector3 prevRandomPos;
    private Vector3 randomAdjustPos;

    void Awake()
    {
        for(int i = 0;i<transform.childCount;i++)
            enemyList.Add(transform.GetChild(i));


        AdjustPos();
        StartCoroutine(EnmeyLiveCheck());
    }

    void AdjustPos()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            while (prevRandomPos == randomAdjustPos)
                randomAdjustPos = new Vector2(0, Random.Range(randomY_min, randomY_max));

            prevRandomPos =   randomAdjustPos;

            enemyList[i].position += randomAdjustPos;
        }
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
