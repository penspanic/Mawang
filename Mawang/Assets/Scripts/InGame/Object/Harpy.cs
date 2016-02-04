﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Harpy : Launcher, ITouchable
{
    [SerializeField]
    private float   buffDuration;
    [SerializeField]
    private float   growthSpeed;
    [SerializeField]
    private Sprite  buff_AS;


    private List<ObjectBase> lineList = new List<ObjectBase>();

    private GameObject  skillEffect;
    private float prevSpeed;

    protected override void Awake()
    {
        base.Awake();
        if (forDecoration)
            return;
        skillEffect     =   transform.FindChild("Effect").gameObject;
        canUseSkill     =   true;
    }



    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            skillEffect.SetActive(true);
            StartCoroutine(BuffRoutine());
        }
    }

    IEnumerator BuffRoutine()
    {
        lineList = battleMgr.ourForceList.FindAll(e => e.line == line &&
            this.attackRange * battleMgr.fightDistance > Mathf.Abs(transform.position.x - e.transform.position.x));

        lineList.Remove(this);
        AddBuffsprRenderer();
        BuffSet(true);
        yield return new WaitForSeconds(buffDuration);
        BuffSet(false);
    }

    void AddBuffsprRenderer()
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            if(lineList[i].transform.FindChild("buff") != null)
            {
                GameObject tmp = lineList[i].transform.FindChild("buff").gameObject;
                tmp.GetComponent<SpriteRenderer>().color               =    Color.white;
                if(tmp.GetComponent<SpriteDelayedDisappear>() == null)
                    tmp.AddComponent<SpriteDelayedDisappear>().delayedTime =    buffDuration;
                continue;
            }
            GameObject go = new GameObject("buff");

            go.AddComponent<SpriteRenderer>().sprite = buff_AS;
            go.AddComponent<SpriteDelayedDisappear>().delayedTime = buffDuration;
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Skill Effect";

            go.transform.SetParent(lineList[i].transform);

            go.transform.localPosition = new Vector2(0, 1.6f);
            AdjustBuffPos(go, lineList[i].name);
        }
    }


    void BuffSet(bool set)
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            if(set)
                lineList[i].GetComponent<Movable>().AddMoveSpeed(growthSpeed);
            else
                lineList[i].GetComponent<Movable>().AddMoveSpeed(-growthSpeed);
        }
    }

    

    public void AdjustBuffPos(GameObject go, string name)
    {
        switch (name)
        {
            case "Dullahan(Clone)":
                go.transform.localPosition -= new Vector3(0.5f, 0.6f);
                break;
            case "Harpy(Clone)":
                go.transform.localPosition += new Vector3(1.1f, 0.5f);
                break;
        }
    }
    public void OnSkillMotionEvent()
    {
        skillEffect.SetActive(false);
        SkillMotionEnd();
    }


}
