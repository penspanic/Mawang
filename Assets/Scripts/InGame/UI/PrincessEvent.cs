using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ===========================
// timeScale이 0일때 움직인다.
// ===========================

public class PrincessEvent : MonoBehaviour
{
    private BgmManager bgmMgr;
    private GameManager gameMgr;
    private Image skillName;
    private Image illust;

    [SerializeField]
    private float moveTime;

    [SerializeField]
    private Vector2 skillNamePos;

    [SerializeField]
    private Vector2 illustPos;

    private Vector2 illustStartPos;
    private Vector2 skillNameStartPos;

    private void Awake()
    {
        bgmMgr = GameObject.FindObjectOfType<BgmManager>();
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        skillName = GameObject.Find("SkillName").GetComponent<Image>();
        illust = GameObject.Find("BigIllust").GetComponent<Image>();
        illustStartPos = new Vector2(1000, illustPos.y);
        skillNameStartPos = new Vector2(-1000, skillNamePos.y);
    }

    // 켜질때
    private void OnEnable()
    {
        Time.timeScale = 0;
        bgmMgr.Pause();
        StartCoroutine(EventProcess());
    }

    private IEnumerator EventProcess()
    {
        if (PlayerData.instance.selectedStage == "C0S1")
        {
            while (TutorialManager.instance.camMove)
                yield return null;
        }

        // 들어오는부분
        yield return StartCoroutine(sprRendererWidthMove(illust, illustStartPos, illustPos));
        yield return StartCoroutine(sprRendererWidthMove(skillName, skillNameStartPos, skillNamePos));

        // 멈추는 부분
        yield return StartCoroutine(WaitForRealSeconds(moveTime + 1f));

        // 나가는 부분
        yield return StartCoroutine(sprRendererWidthMove(illust, illustPos, illustStartPos));
        yield return StartCoroutine(sprRendererWidthMove(skillName, skillNamePos, skillNameStartPos));

        // 비활성화
        Time.timeScale = gameMgr.userTimeScale;
        bgmMgr.Resume();
        gameObject.SetActive(false);
    }

    private IEnumerator sprRendererWidthMove(Image sprRenderer, Vector2 start, Vector2 end)
    {
        float beginTime = Time.unscaledTime;

        while (Time.unscaledTime - beginTime <= moveTime)
        {
            float t = (Time.unscaledTime - beginTime) / moveTime;

            float x = EasingUtil.easeOutCirc(start.x, end.x, t);

            sprRenderer.transform.localPosition = new Vector2(x, sprRenderer.transform.localPosition.y);

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