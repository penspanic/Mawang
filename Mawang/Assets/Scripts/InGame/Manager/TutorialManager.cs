using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    List<Sprite> tutorialSpriteList = new List<Sprite>();
    List<Sprite> effectSpriteList = new List<Sprite>();

    [SerializeField]
    private float twinkleDelay = 0.5f;

    private SpawnManager spawnMgr;
    private BattleManager battleMgr;
    private SelectTab selectTab;
    private BgmManager bgmMgr;

    private Movable skeleton;
    private Image tutorialsprRenderer;
    private Image effectsprRenderer;

    private int[] effectIdx = { 1, 5 };
    private int currIdx = -1;
    private int maxCount = 10;

    private int effectFindIdx = 0;

    public int EffectFindIdx
    {
        get
        {
            return effectFindIdx++;
        }
    }
    public bool isRunning { get; private set; }


    public void Init()
    {
        Time.timeScale = 0;


        spawnMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<SpawnManager>();
        battleMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<BattleManager>();
        bgmMgr = GameObject.FindGameObjectWithTag("Manager").GetComponent<BgmManager>();

        selectTab = GameObject.Find("SelectTab").GetComponent<SelectTab>();

        for (int i = 1; i <= 10; i++)
        {
            tutorialSpriteList.Add(
                Resources.Load<Sprite>("Sprite/Tutorial/" + i));
        }

        skeleton = Resources.Load<Movable>("Prefabs/OurForce/Skeleton");

        for (int i = 0; i < effectIdx.Length; i++)
        {
            effectSpriteList.Add(Resources.Load<Sprite>("Sprite/Tutorial/" + effectIdx[i] + "_1"));
            effectSpriteList.Add(Resources.Load<Sprite>("Sprite/Tutorial/" + effectIdx[i] + "_2"));
        }



        tutorialsprRenderer = CreatesprRendererObj("TutorialObj");
        effectsprRenderer = CreatesprRendererObj("TwinkleObj");

        bgmMgr.Pause();

        NextStep();
        StartCoroutine(TutorialProcess());
    }


    IEnumerator TutorialProcess()
    {
        isRunning = true;
        while (currIdx < maxCount)
        {
            InputProcess();
            yield return null;
        }

        // end Processing

        isRunning = false;
        Destroy(tutorialsprRenderer);
        Destroy(effectsprRenderer);
        Time.timeScale  =   1;
        yield break;
    }

    void InputProcess()
    {
        if (Input.GetMouseButtonDown(0))
            NextStep();
    }



    void NextStep()
    {
        currIdx++;
        if (currIdx == maxCount)
        {
            StartCoroutine(selectTab.RotateSelectTab());
            bgmMgr.Resume();
            return;
        }

        if (currIdx == 0 || currIdx == 4)
        {
            effectsprRenderer.enabled = true;
            StartCoroutine(Twinkle(
                effectSpriteList[EffectFindIdx], effectSpriteList[EffectFindIdx]));
        }
        else
            effectsprRenderer.enabled = false;

        if (currIdx == 3)
        {
            spawnMgr.SpawnOurForce(skeleton, 1);
            StartCoroutine(DropUnit());
        }
        if (currIdx == 7)
            StartCoroutine(selectTab.RotateSelectTab());

        tutorialsprRenderer.sprite = tutorialSpriteList[currIdx];

    }


    float dropDuration = 0.6f;
    IEnumerator DropUnit()
    {
        float beginTime = Time.unscaledTime;

        while (Time.unscaledTime - beginTime <= dropDuration)
        {
            float t = (Time.unscaledTime - beginTime) / dropDuration;

            float y = EasingUtil.easeOutQuad(4.5f, 0, t);

            battleMgr.ourForceList[0].transform.position = new Vector2(0, y);

            yield return null;
        }

        battleMgr.ourForceList[0].transform.position = new Vector2(0, 0);

        yield break;
    }


    IEnumerator Twinkle(Sprite one, Sprite two)
    {
        bool sw = true;
        effectsprRenderer.enabled = true;
        while (currIdx == 0 || currIdx == 4)
        {
            if (sw)
                effectsprRenderer.sprite = one;
            else
                effectsprRenderer.sprite = two;
            sw = !sw;

            yield return StartCoroutine(WaitForRealSeconds(twinkleDelay));
        }

        yield break;

    }


    public IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }


    Image CreatesprRendererObj(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(GameObject.Find("Canvas").transform);
        go.AddComponent<RectTransform>();
        go.AddComponent<Image>();

        go.GetComponent<RectTransform>().sizeDelta = new Vector2(1280, 720);
        go.GetComponent<RectTransform>().localScale = Vector3.one;
        return go.GetComponent<Image>();
    }
}
