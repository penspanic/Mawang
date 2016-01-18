using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour
{
    public Animator logoAnimator;

    void Awake()
    {
        StartCoroutine(LogoProcess());
    }

    IEnumerator LogoProcess()
    {
        logoAnimator.enabled = true;
        yield return new WaitForSeconds(2f);

        StartCoroutine(SceneFader.Instance.FadeOut(1.5f, "Title"));
    }
}
