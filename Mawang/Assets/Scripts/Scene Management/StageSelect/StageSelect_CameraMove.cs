using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StageSelect_CameraMove : MonoBehaviour
{
    StageSelect stageSelect;

    RectTransform targetTransform;

    RectTransform[][] stagesTransform = new RectTransform[4][];
    void Awake()
    {
        stageSelect = GameObject.FindObjectOfType<StageSelect>();

        string stageName;
        for (int i = 0; i < 4; i++)
        {
            List<RectTransform> currChapterStageTransformList = new List<RectTransform>();
            for (int j = 0; j < 3; j++)
            {
                stageName = "C" + i.ToString() + "S" + (j + 1).ToString();
                currChapterStageTransformList.Add(
                    GameObject.Find(stageName).GetComponent<RectTransform>());

            }
            stagesTransform[i] = currChapterStageTransformList.ToArray();
        }
        targetTransform = stagesTransform[0][0];
    }

    public void MoveButtonDown(bool moveRight)
    {

    }
}
