using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ===========================
// timeScale이 0일때 움직인다.
// ===========================

public class PrincessEvent : MonoBehaviour 
{
    private BgmManager  bgmMgr;
    private Image       skillName;
    private Image       illust;

    [SerializeField]
    float   moveTime;
    [SerializeField]
    Vector2 skillNamePos;
    [SerializeField]
    Vector2 illustPos;
    


    private Vector2     illustStartPos;
    private Vector2     skillNameStartPos;

    void Awake()
    {
        bgmMgr      = GameObject.FindGameObjectWithTag("Manager").GetComponent<BgmManager>();
        skillName   = GameObject.Find("SkillName").GetComponent<Image>();
        illust      = GameObject.Find("BigIllust").GetComponent<Image>();

        illustStartPos = new Vector2(1000, illustPos.y);
        skillNameStartPos = new Vector2(-1000, skillNamePos.y);
    }
    // 켜질때
    void OnEnable()
    {       
        Time.timeScale      =   0;
        bgmMgr.Pause();
        StartCoroutine(EventProcess());
    }


    IEnumerator EventProcess()
    {
        // 들어오는부분
        StartCoroutine(sprRendererWidthMove(illust, illustStartPos, illustPos));
        yield return StartCoroutine(WaitForRealSeconds(moveTime + 0.2f));
        StartCoroutine(sprRendererWidthMove(skillName, skillNameStartPos, skillNamePos));

        // 멈추는 부분 
        yield return StartCoroutine(WaitForRealSeconds(moveTime + 1f));

        // 나가는 부분 
        StartCoroutine(sprRendererWidthMove(illust, illustPos, illustStartPos));
        yield return StartCoroutine(WaitForRealSeconds(moveTime + 0.2f));
        StartCoroutine(sprRendererWidthMove(skillName, skillNamePos, skillNameStartPos));
        yield return StartCoroutine(WaitForRealSeconds(moveTime + 0.2f));


        // 비활성화
        Time.timeScale  =   1;
        bgmMgr.Resume();
        gameObject.SetActive(false);

    }


    IEnumerator sprRendererWidthMove(Image sprRenderer, Vector2 start, Vector2 end)
    {
        float beginTime = Time.unscaledTime;

        while (Time.unscaledTime - beginTime <= moveTime )
        {
            float t =   (Time.unscaledTime - beginTime) / moveTime;

            float x =   EasingUtil.easeOutCirc(start.x,end.x,t);

            sprRenderer.transform.localPosition = new Vector2(x,sprRenderer.transform.localPosition.y);

            yield return null;
        }
        // 보정
        sprRenderer.transform.localPosition = end;
    }

    public IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
