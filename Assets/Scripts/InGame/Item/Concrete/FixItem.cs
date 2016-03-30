using UnityEngine;
using System.Collections;
using System;
public class FixItem : ItemBase
{
    SatanCastle satanCastle;
    float hpHealRate = 0.3f;

    
    protected override void Awake()
    {
        base.Awake();
        satanCastle = GameObject.FindObjectOfType<SatanCastle>();
        message = "마왕성의 체력이 회복됩니다.";
    }

    protected override void Useitem()
    {
        PlayerData.instance.UseItem(name);
        amount--;
        msgBox.PushMessage(message);
        satanCastle.UseFixItem(hpHealRate);
    }
}