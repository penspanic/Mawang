using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteOrderLayerManager : MonoBehaviour
{
    int order           =   0;
    int orderInterval   =   6;
    Queue<int> deathOrderQueue = new Queue<int>();

    
    public int Order
    {
        get
        {
            if (deathOrderQueue.Count > 0)
            {
                return deathOrderQueue.Dequeue();
            }
            return order += orderInterval;
        }
    }

    public void Reset()
    {
        this.order = 0;
    }

    public void AddDeathOrder(int order)
    {
        deathOrderQueue.Enqueue(order);
    }

    public int SetSpriteOrder(SpriteRenderer[] sprs)
    {
        int temp = Order;
        for (int i = 0; i < sprs.Length; i++)
        {
            sprs[i].sortingOrder += temp;
        }
        return temp;
    }
}
