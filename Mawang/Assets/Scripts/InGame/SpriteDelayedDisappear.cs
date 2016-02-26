﻿using UnityEngine;
using System.Collections;

public delegate void CallbackDelegate(GameObject obj);
public class SpriteDelayedDisappear : MonoBehaviour
{
    public CallbackDelegate callBack;
    public float delayedTime;
    public float duration;
    public bool isDestory;
    SpriteRenderer spr;

    void Awake()
    {
        spr =   GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(delayedTime);

        float currTime  =   0.0f;
        while (currTime < duration)
        {
            currTime    +=  Time.deltaTime;

            float alpha =   EasingUtil.easeOutExpo(1, 0, currTime / duration);

            spr.color   =   new Color(1, 1, 1, alpha);
            yield return null;
        }
        
        spr.color       =   new Color(0, 0, 0, 0);

        if (callBack != null)   
            callBack(gameObject);
        else
        {
            if (isDestory)
                DestroyObject(this);
            else
                gameObject.SetActive(false);
        }
        

        yield break;

    }
}
