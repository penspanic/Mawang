using UnityEngine;
using System.Collections;

public class StageSelect_MapScroll : MonoBehaviour
{
    bool prevMouseDown = false;

    Vector2 prevTouchPos = Vector2.zero;
    Vector2 currTouchPos = Vector2.zero;
    Vector2 deltaPos = Vector2.zero;

    void Update()
    {
        if (Input.touchCount == 2)
            return;
        currTouchPos = Input.mousePosition;

        deltaPos = prevTouchPos - currTouchPos;
        prevTouchPos = currTouchPos;

        if (Input.GetMouseButton(0) && prevMouseDown)
        {
            float moveValue = deltaPos.x * Time.deltaTime;

            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(newPos.x + moveValue, 0, GetMaxPosX());
            transform.position = newPos;

            prevMouseDown = true;
        }
        if (Input.GetMouseButton(0))
            prevMouseDown = true;
        else
            prevMouseDown = false;
    }

    float GetMaxPosX()
    {
        return 12.8f;
    }

    float GetMaxPosY()
    {
        return 7.2f;
    }
}
