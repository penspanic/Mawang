﻿using UnityEngine;
using System.Collections;

public class VirtualLine : MonoBehaviour, ITouchable
{
    SpawnManager    spawnMgr;
    SelectTab       selectTab;
    Movable         spawnObj;

    void Awake()
    {
        selectTab   =   FindObjectOfType<SelectTab>();
        spawnMgr    =   FindObjectOfType<SpawnManager>();
    }



    public void OnTouch()
    {
        spawnObj    =   selectTab.GetUnit();

        if (spawnObj != null)
        {
            switch (gameObject.name)
            {
                case "Spawn Line1":
                    spawnMgr.TrySpawnOurForce(spawnObj, 1);
                    break;
                case "Spawn Line2":
                    spawnMgr.TrySpawnOurForce(spawnObj, 2);
                    break;
                case "Spawn Line3":
                    spawnMgr.TrySpawnOurForce(spawnObj, 3);
                    break;
            }
        }
        selectTab.ResetButton();
    }
}
