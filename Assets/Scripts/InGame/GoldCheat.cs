using UnityEngine;
using System.Collections;

public class GoldCheat : MonoBehaviour
{
    int touchCount = 0;

    GoldManager goldMgr;

    void Awake()
    {
        goldMgr = GameObject.FindObjectOfType<GoldManager>();
    }

    public void OnCheatButtonDown()
    {
        touchCount++;

        if(touchCount >= 10)
        {
            goldMgr.AddGold(1000);
            touchCount = 0;
        }
    }
}
