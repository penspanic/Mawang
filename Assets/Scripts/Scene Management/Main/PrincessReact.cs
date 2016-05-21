using System.Collections.Generic;
using UnityEngine;

public class PrincessReact : MonoBehaviour
{
    private RectTransform[][] princessTransforms;
    private bool[][] princessExists;

    private Main_Princess[] princesses;

    private void Awake()
    {
        PlayerData.instance.CheckInstance();
        JsonManager.instance.CheckInstance();
        LoadPosition();
        CreatePrincesses();
        PrincessPosSetting();
    }

    private void LoadPosition()
    {
        princessTransforms = new RectTransform[2][];
        princessExists = new bool[2][];

        List<RectTransform> lineTransformList = new List<RectTransform>();

        for (int i = 0; i < 2; i++)
        {
            lineTransformList.Clear();
            GameObject lineParent = GameObject.Find("Line " + (i + 1).ToString());
            for (int j = 0; j < lineParent.transform.childCount; j++)
            {
                lineTransformList.Add(lineParent.transform.GetChild(j).GetComponent<RectTransform>());
            }
            princessTransforms[i] = lineTransformList.ToArray();
            princessExists[i] = new bool[lineTransformList.Count];
        }
    }

    private void CreatePrincesses()
    {
        int lastChapter = PlayerData.instance.GetClearedLastChapter();
        if (lastChapter == 999)
            return;
        List<Main_Princess> princessList = new List<Main_Princess>();
        Main_Princess newPrincess;
        for (int i = 0; i <= lastChapter; i++)
        {
            newPrincess = Instantiate(Resources.Load<Main_Princess>("Prefabs/Main/C" + i.ToString() + " Main Princess"));
            newPrincess.SetScripts(JsonManager.instance.GetPrincessScript("C" + i.ToString()));
            princessList.Add(newPrincess);
        }
        princesses = princessList.ToArray();
    }

    private void PrincessPosSetting()
    {
        if (princesses == null)
            return;
        for (int i = 0; i < princesses.Length; i++)
        {
            while (true)
            {
                int randomX = Random.Range(0, 3);
                int randomY = Random.Range(0, 2);
                if (princessExists[randomY][randomX])
                {
                    continue;
                }
                else
                {
                    princessExists[randomY][randomX] = true;
                    princesses[i].transform.SetParent(princessTransforms[randomY][randomX], false);
                    princesses[i].transform.localPosition = Vector2.zero;
                    break;
                }
            }
        }
    }
}