using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelect_Background : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static bool touched;

    public static bool Touched()
    {
        return touched;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touched = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touched = false;
    }
}