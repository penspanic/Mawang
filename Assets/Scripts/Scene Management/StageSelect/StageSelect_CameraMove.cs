using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StageSelect_CameraMove : MonoBehaviour
{
    public bool isMoving;
    public Button leftButton;
    public Button rightButton;
    float moveTime = 2f;

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

        currStopIndex = PlayerData.instance.stageSelectPosIndex;
        this.transform.position = cameraStopTransform[currStopIndex].position;
        this.transform.Translate(Vector3.forward * -10);

        ButtonStateSet();
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
            currPos = EasingUtil.EaseVector2(
                EasingUtil.easeInOutQuart, moveStartPos, targetTransform.position, elaspedTime / moveTime);
            currPos.z = -10;
            this.transform.position = currPos;
            yield return null;
        }
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
        PlayerData.instance.stageSelectPosIndex = currStopIndex;
        targetTransform = cameraStopTransform[currStopIndex];

        ButtonStateSet();
        StartCoroutine(MoveCamera());
    }

    bool IsArrived(RectTransform target)
    {
        Vector2 cameraPos = this.transform.position;
        Vector2 targetPos = target.transform.position;

        float distance = (cameraPos - targetPos).magnitude;
        if (distance < 0.1f)
            return true;
        else
            return false;
    }

    void ButtonStateSet()
    {
        if(currStopIndex == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
        else if(currStopIndex == 3)
        {
            leftButton.interactable = true;
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }
}