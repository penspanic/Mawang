using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StageSelect_CameraMove : MonoBehaviour
{
    public bool isMoving;

    float moveTime = 3f;

    StageSelect stageSelect;

    RectTransform targetTransform;
    RectTransform[] cameraStopTransform;

    int currStopIndex = 0;
    void Awake()
    {
        stageSelect = GameObject.FindObjectOfType<StageSelect>();

        List<RectTransform> cameraStopTransformList = new List<RectTransform>();
        for (int i = 0; i < 4; i++)
        {
            cameraStopTransformList.Add(GameObject.Find("Chapter" + i.ToString()).GetComponent<RectTransform>());
        }
        cameraStopTransform = cameraStopTransformList.ToArray();
    }

    IEnumerator MoveCamera()
    {
        isMoving = true;
        Vector2 moveStartPos = this.transform.position;

        float elaspedTime = 0f;
        while(!IsArrived(targetTransform))
        {
            elaspedTime += Time.deltaTime;
            Vector3 currPos;
            currPos.x = EasingUtil.easeInOutQuart(moveStartPos.x, targetTransform.position.x, elaspedTime / moveTime);
            currPos.y = EasingUtil.easeInOutQuart(moveStartPos.y, targetTransform.position.y, elaspedTime / moveTime);
            currPos.z = -10;
            this.transform.position = currPos;
            yield return null;
        }
        Debug.Log(elaspedTime);
        isMoving = false;
    }

    public void MoveButtonDown(bool moveRight)
    {
        if (isMoving)
            return;
        if (currStopIndex == 0 && !moveRight)
            return;
        else if (currStopIndex == 3 && moveRight)
            return;

        currStopIndex += moveRight ? 1 : -1;
        targetTransform = cameraStopTransform[currStopIndex];

        StartCoroutine(MoveCamera());
    }

    bool IsArrived(RectTransform target)
    {
        Vector2 cameraPos = this.transform.position;
        Vector2 targetPos = target.transform.position;

        float distance = (cameraPos - targetPos).magnitude;
        Debug.Log(distance);
        if (distance < 0.1f)
            return true;
        else
            return false;
    }
}
