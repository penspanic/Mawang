using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class StageSelect_Background : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool backgroundTouched
    {
        get;
        private set;
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundTouched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backgroundTouched = false;
    }
}
