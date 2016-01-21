using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public float randomY_Min;
    public float randomY_Max;

    private GameManager gameMgr;
    private GoldManager goldMgr;
    private SpriteOrderLayerManager orderMgr;
    private GameObject satanCastle;

    private List<Vector2> ourForceLinePos   =   new List<Vector2>();

    private Vector2 randomY;

    void Awake()
    {
        gameMgr     = FindObjectOfType<GameManager>();
        goldMgr     = FindObjectOfType<GoldManager>();
        orderMgr    = FindObjectOfType<SpriteOrderLayerManager>();
        satanCastle = GameObject.Find("SatanCastle");

        for (int i = 1; i <= 3; i++)
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
        Instantiate(obj, ourForceLinePos[line - 1] + randomY, new Quaternion());
        orderMgr.UpdateOrder(line);
    }

}
