using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public enum ButtonEffectType
{
    Expand,
    BigAndSmall,
}
public class ButtonEffect : MonoBehaviour
{
    public ButtonEffectType type;
    public bool childChange;

    Button targetButton;
    EventTrigger trigger;
    Vector2 originalSize;
    RectTransform rectTransform;
    void Awake()
    {
        targetButton = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();

        trigger = targetButton.gameObject.AddComponent<EventTrigger>();

        AddEventTrigger(EventTriggerType.PointerEnter, OnPointerEnter);
    }

    void AddEventTrigger(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry;
        entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    public void OnPointerEnter(BaseEventData data)
    {
        // Filter child object's event out.
        if ((data as PointerEventData).pointerEnter != targetButton.gameObject) 
            return;

        if (!isMoving)
        {
            switch(type)
            {
                case ButtonEffectType.Expand:
                    StartCoroutine(Expand(0.25f, childChange));
                    break;
                case ButtonEffectType.BigAndSmall:
                    StartCoroutine(BigAndSmall(0.785f, childChange));
                    break;
            }
        }
    }

    void OnDisable()
    {
        if(isMoving)
        {
            isMoving = false;
            rectTransform.sizeDelta = originalSize;
            StopAllCoroutines();
        }
    }

    bool isMoving = false;


    IEnumerator Expand(float time, bool childChange = false)
    {
        isMoving = true;
        float elapsedTime = 0f;

        if (childChange)
            originalSize = GetComponent<RectTransform>().localScale;
        else
            originalSize = GetComponent<RectTransform>().sizeDelta;

        Vector3 startSize = originalSize * 0.66f;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            if (childChange)
                rectTransform.localScale = EasingUtil.EaseVector3(EasingUtil.smoothstep, startSize, originalSize, elapsedTime / time);
            else
                rectTransform.sizeDelta = EasingUtil.EaseVector3(EasingUtil.smoothstep, startSize, originalSize, elapsedTime / time);

            yield return null;
        }
        if (childChange)
            rectTransform.localScale = originalSize;
        else
            rectTransform.sizeDelta = originalSize;
        isMoving = false;
    }

    IEnumerator BigAndSmall(float time, bool childChange = false)
    {
        isMoving = true;
        float elapsedTime = 0f;

        if (childChange)
            originalSize = rectTransform.localScale;
        else
            originalSize = rectTransform.sizeDelta;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            if (childChange)
                rectTransform.localScale = originalSize + (originalSize * Mathf.Sin(elapsedTime * 20)) / (elapsedTime * 5 + 5);
            else
                rectTransform.sizeDelta = originalSize + (originalSize * Mathf.Sin(elapsedTime * 20)) / (elapsedTime * 5 + 5);

            yield return null;
        }

        if (childChange)
            rectTransform.localScale = originalSize;
        else
            rectTransform.sizeDelta = originalSize;
        isMoving = false;
    }


   
}
