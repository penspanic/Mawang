﻿using UnityEngine;

public class StageSelect_MapScroll : MonoBehaviour
{
    private StageSelect_Background background;
    private StageSelect_PinchZoom pinchZoom;

    private bool prevMouseDown = false;

    private Vector2 prevTouchPos = Vector2.zero;
    private Vector2 currTouchPos = Vector2.zero;
    private Vector2 deltaPos = Vector2.zero;

    private void Awake()
    {
        background = GameObject.FindObjectOfType<StageSelect_Background>();
        pinchZoom = GameObject.FindObjectOfType<StageSelect_PinchZoom>();
    }

    private void Update()
    {
        if (!StageSelect_Background.Touched() || Input.touchCount >= 2)
        {
            prevMouseDown = false;
            return;
        }
        currTouchPos = Input.mousePosition;

        deltaPos = prevTouchPos - currTouchPos;
        prevTouchPos = currTouchPos;

        if (Input.GetMouseButton(0) && prevMouseDown)
        {
            Vector2 moveValue = deltaPos * Time.deltaTime;

            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(newPos.x + moveValue.x, GetMinPosX(), GetMaxPosX());
            newPos.y = Mathf.Clamp(newPos.y + moveValue.y, GetMinPosY(), GetMaxPosY());
            transform.position = newPos;

            prevMouseDown = true;
        }
        if (Input.GetMouseButton(0))
            prevMouseDown = true;
        else
            prevMouseDown = false;
    }

    private float GetMaxPosX()
    {
        return 12.8f + (1f - pinchZoom.ratio) * 6.4f;
    }

    private float GetMinPosX()
    {
        return 0f - (1f - pinchZoom.ratio) * 6.4f;
    }

    private float GetMaxPosY()
    {
        return 7.2f + (1f - pinchZoom.ratio) * 3.6f;
    }

    private float GetMinPosY()
    {
        return 0f - (1f - pinchZoom.ratio) * 3.6f;
    }
}