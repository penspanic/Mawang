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

        Debug.Log("Pointer Enter");
        if (!isMoving)
        {
            switch(type)
            {
                case ButtonEffectType.Expand:
                    StartCoroutine(Expand(0.25f));
                    break;
                case ButtonEffectType.BigAndSmall:
                    StartCoroutine(BigAndSmall(0.785f));
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


    IEnumerator Expand(float time)
    {
        isMoving = true;
        float elapsedTime = 0f;

        originalSize = GetComponent<RectTransform>().sizeDelta;

        Vector2 startSize = originalSize / 2;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            rectTransform.sizeDelta = EasingUtil.EaseVector2(EasingUtil.smoothstep, startSize, originalSize, elapsedTime / time);

            yield return null;
        }
        rectTransform.sizeDelta = originalSize;
        isMoving = false;
    }
    IEnumerator BigAndSmall(float time)
    {
        float elapsedTime = 0f;

        originalSize = GetComponent<RectTransform>().sizeDelta;
        isMoving = true;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            rectTransform.sizeDelta = originalSize + (originalSize * Mathf.Sin(elapsedTime * 20)) / (elapsedTime * 5 + 5);

            yield return null;
        }
        rectTransform.sizeDelta = originalSize;
        isMoving = false;
    }


   
}
