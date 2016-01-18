using UnityEngine;
using System.Collections;

public class SpriteDelayedDisappear : MonoBehaviour
{
    public float delayedTime;
    private float duration;

    SpriteRenderer spr;

    void Awake()
    {
        spr =   GetComponent<SpriteRenderer>();
        duration    =   1;
    }

    void Start()
    {
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(delayedTime);

        float currTime  =   0.0f;
        Debug.Log("currTime: "+currTime);
        Debug.Log("duration: "+duration);
        while (currTime < duration)
        {
            Debug.Log(duration);
            Debug.Log(currTime);
            currTime    +=  Time.deltaTime;

            float alpha =   EasingUtil.easeOutExpo(1, 0, currTime / duration);

            spr.color   =   new Color(1, 1, 1, alpha);
            yield return null;
        }
        
        spr.color       =   new Color(0, 0, 0, 0);
        DestroyObject(this);
        yield break;

    }
}
