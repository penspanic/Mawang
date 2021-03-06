﻿using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float randomY_Min;
    public float randomY_Max;
    
    private GameManager gameMgr;
    private GoldManager goldMgr;
    private SpriteOrderLayerManager orderMgr;
    private GameObject satanCastle;

    private List<Vector2> ourForceLinePos = new List<Vector2>();

    private void Awake()
    {
        gameMgr = FindObjectOfType<GameManager>();
        goldMgr = FindObjectOfType<GoldManager>();
        orderMgr = FindObjectOfType<SpriteOrderLayerManager>();
        satanCastle = GameObject.Find("SatanCastle");

        for (int i = 1; i <= 3; ++i)
        {
            ourForceLinePos.Add(satanCastle.transform.FindChild(string.Format("Spawn Line{0}", i)).position);
        }
    }

    public void TrySpawnOurForce(Movable obj, int line)
    {
        if (goldMgr.playerGold - obj.GetUnitCost() >= 0)
        {
            goldMgr.playerGold -= obj.GetUnitCost();
            SpawnOurForce(obj, line);
        }
    }

    public void SpawnOurForce(Movable obj, int line)
    {
        Instantiate(obj, ourForceLinePos[line - 1], new Quaternion());
        orderMgr.UpdateOrder(line);
    }

    public Movable SpawnOurForce(Movable obj, int line, Vector2 pos)
    {
        Movable newObj = Instantiate(obj, ourForceLinePos[line - 1], new Quaternion()) as Movable;
        orderMgr.UpdateOrder(line);
        newObj.transform.position = pos;

        return newObj;
    }
}