using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ButtonEffect : MonoBehaviour
{
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
            StartCoroutine(ScaleMove(0.785f));
    }

    bool isMoving = false;
    IEnumerator ScaleMove(float time)
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
        isMoving = false;
        rectTransform.sizeDelta = originalSize;
    }
}
