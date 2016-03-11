using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum TutorialEvent
{
    PrepareGame,
    CastleFullGauge,
    PrincessFullGauge
}


/// <summary>
/// 튜토리얼 관리자
/// 적절한시기에 맞는 튜토리얼을 실행시키면된다.
/// </summary>
public class TutorialManager : Singleton<TutorialManager>
{

    public Dictionary<TutorialEvent, List<Sprite>> tutorialDic { get; private set; }

    public bool isPlaying { get; private set; } // 실행중일때
    public bool camMove { get; private set; }
    public int PatternCnt { get; set; }     // 유닛 C0S1일때 초반에만 많이

    public bool onceCastleTuto { get; set; }
    public bool oncePrinTuto { get; set; }

    [SerializeField]
    private GameObject   tutorialPrefab;

    private GameObject   tutorialObj;

    private List<Sprite> preTutoList;       // 겜 시작전 스프라이트
    private List<Sprite> preTutoEffList;    //   "    이펙트
    private List<Sprite> castleTutoList;    // 성 풀게이지 스프라이트
    private List<Sprite> princessTutoList;  // 공주 풀게이지 스프라이트


    private Texture2D spriteTexture;
    private CanvasGroup canvasGroup;
    private Image mainImg;
    private Image effectImg;

    private GoldManager goldMgr;
    private SpawnManager spawnMgr;
    private BattleManager battleMgr;
    private SelectTab   selectTab;
    private Movable  skeleton;
    private SatanCastle castle;
    private Vector3 mousePos = Vector3.zero;

    void Awake()
    {
        tutorialDic         =   new Dictionary<TutorialEvent,List<Sprite>>();
        preTutoList         =   new List<Sprite>();
        preTutoEffList      =   new List<Sprite>();
        castleTutoList      =   new List<Sprite>();
        princessTutoList    =   new List<Sprite>();
        

        // 튜토리얼 관련 obj 초기화
        tutorialObj = GameObject.Instantiate(tutorialPrefab);
        tutorialObj.transform.SetParent(GameObject.Find("Canvas").transform);
        tutorialObj.transform.localScale = Vector3.one;

        // 튜토리얼 obj 하위오브젝트 연결
        mainImg             =   tutorialObj.transform.FindChild("TutoImg").GetComponent<Image>();
        effectImg           =   tutorialObj.transform.FindChild("EffectImg").GetComponent<Image>();
        canvasGroup         =   tutorialObj.GetComponent<CanvasGroup>();

        tutorialObj.SetActive(false);

        // 이미지들 추가
        AddImgListFromFolder(preTutoList, "Prepare");
        AddImgListFromFolder(preTutoEffList, "Prepare_Effect");
        AddImgListFromFolder(princessTutoList,"PrincessFullGauge");
        AddImgListFromFolder(castleTutoList, "CastleFullGauge");

        // dic에 추가
        tutorialDic.Add(TutorialEvent.PrepareGame, preTutoList);
        tutorialDic.Add(TutorialEvent.PrincessFullGauge, princessTutoList);
        tutorialDic.Add(TutorialEvent.CastleFullGauge, castleTutoList);


        goldMgr     =   GameObject.Find("Manager").GetComponent<GoldManager>();
        selectTab   =   GameObject.Find("SelectTab").GetComponent<SelectTab>();
        spawnMgr    =   GameObject.Find("Manager").GetComponent<SpawnManager>();
        battleMgr   =   GameObject.Find("Manager").GetComponent<BattleManager>();
        castle      =   GameObject.Find("SatanCastle").GetComponent<SatanCastle>();

        skeleton    =   Resources.Load<Movable>("Prefabs/OurForce/Skeleton");
        camMove     =   false;
        isPlaying   =   false;
        onceCastleTuto  = true;
        oncePrinTuto    = true;
        PatternCnt  =   6;
    }

    void AddImgListFromFolder(List<Sprite> imgList, string folderName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Tutorial/" + folderName);

        for(int i = 0; i < sprites.Length; i++)
            imgList.Add(sprites[i]);
    }

    public void PlayTutorial(TutorialEvent tutorial)
    {
        tutorialObj.SetActive(true);
        isPlaying = true;
        StartCoroutine(CheckTutorial(tutorial));
    }

    IEnumerator CheckTutorial(TutorialEvent tutorial)
    {
        int tutoIdx = 0;
        int effectIdx = 0;
        bool isAnyTouch =   false;
        bool isTouch    =   false;

        while (tutoIdx < tutorialDic[tutorial].Count)
        {
            mainImg.sprite  =   tutorialDic[tutorial][tutoIdx];
            spriteTexture   =   mainImg.sprite.texture;
            isAnyTouch      =   tutorialDic[tutorial][tutoIdx].name.Length == 2 ? true : false;
            isTouch         =   false;

            #region cases

            #region case of tutorial

            // 튜토리얼이 preparegame 일때
            if (TutorialEvent.PrepareGame == tutorial)
            {


                // 이펙트 idx 이름과 튜토리얼 idx 이름이 같을때 (지금은 한자리수만)
                if (preTutoEffList[effectIdx].name[0] == tutorialDic[tutorial][tutoIdx].name[0])
                {
                    effectImg.sprite = preTutoEffList[effectIdx];
                    effectImg.gameObject.SetActive(true);
                }

                switch (tutoIdx)
                {
                    case 0: // 제일 첨
                        goldMgr.AddGold(50);
                        break;
                    case 2: // 스켈레톤 누를때 
                        selectTab.ClikcedUnitButton(0);
                        break;
                    case 3: // 라인누를때
                        spawnMgr.TrySpawnOurForce(skeleton, 2);
                        selectTab.ResetButton();
                        //  selectTab.LineSetActive(false);

                        Time.timeScale = 1;
                        while (Time.timeScale == 1)
                        {
                            if (battleMgr.ourForceList[0].transform.position.x >= -0.8f)
                                Time.timeScale = 0;
                            yield return null;
                        }
                        break;
                    case 4: // 스켈레톤 누를때
                        Time.timeScale = 1;
                        float currTime = 0.0f;
                        while (Time.timeScale == 1)
                        {
                            // x 값 고정
                            battleMgr.ourForceList[0].transform.position = new Vector2(-0.8f, battleMgr.ourForceList[0].transform.position.y);
                            battleMgr.ourForceList[0].GetComponent<Skeleton>().OnTouch();

                            currTime += Time.deltaTime;
                            if (currTime >= 0.9f)
                                Time.timeScale = 0;

                            yield return null;
                        }
                        break;
                    case 6:
                        StartCoroutine(selectTab.RotateSelectTab());
                        break;
                    case 7:
                        camMove = true;
                        while (!isTouch)
                        {
                            if (Camera.main.transform.position.x >= 10)
                            {
                                isTouch = true;
                                tutoIdx++;
                            }
                            yield return null;
                        }
                        camMove = false;
                        break;
                }

            }
            #endregion

            #region case of castleTuto

            if (TutorialEvent.CastleFullGauge == tutorial)
            {
                switch (tutoIdx)
                {
                    case 0:
                        Time.timeScale = 0;
                        StartCoroutine(MoveCam(0,1.0f));

                        break;
                    case 1:
                        Time.timeScale = 1;
                        castle.OnTouch();
                        break;
                }
            }

            #endregion

            #region case of prin

            if (TutorialEvent.PrincessFullGauge == tutorial)
            {
                switch(tutoIdx)
                {
                    case 0:
                        Time.timeScale = 0;
                        camMove = true;

                        break;
                    case 1:
                        camMove = false;
                        Time.timeScale = 1;

                        // float currTime = 0;
                        //while (Time.timeScale == 1)
                        //{
                        //    currTime += Time.deltaTime;
                        //    if (currTime >= 2f)
                        //        Time.timeScale = 1;

                        //    yield return null;
                        //}
                        break;
                }
            }

            #endregion

            #endregion

            while (!isTouch)
            {
                if (Input.GetMouseButtonDown(0) && OnPointerDown(isAnyTouch))
                {
                    isTouch = true;
                    if (effectImg.gameObject.activeInHierarchy)
                    {
                        effectIdx++;
                        effectImg.gameObject.SetActive(false);
                    }
                    tutoIdx++;
                }
                yield return null;
            }
            canvasGroup.blocksRaycasts = true;
            yield return null;
        }

        Time.timeScale = 1;
        tutorialObj.SetActive(false);

        if(tutorial == TutorialEvent.PrepareGame)
            StartCoroutine(selectTab.RotateSelectTab());

        isPlaying = false;
    }



    
    bool OnPointerDown(bool anyTouch)
    {
        mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (!anyTouch)
        {
            // 투명찾기
            if (spriteTexture.GetPixel((int)(1280f * mousePos.x), (int)(720f * mousePos.y)).a == 0)
                return true;
            else
                return false;
        }
        else
        return true;
    }


    IEnumerator MoveCam(float endX, float moveTime)
    {
        float currTime = 0.0f;

        float startX = Camera.main.transform.position.x;
        while (currTime < moveTime)
        {
            currTime += EasingUtil.tick;
            float x = EasingUtil.smoothstep(startX, endX, currTime / moveTime);
            Camera.main.transform.position = new Vector3(x, Camera.main.transform.position.y,-10);
            yield return null;
        }

        Camera.main.transform.position = new Vector3(endX, Camera.main.transform.position.y,-10);
    }
}
