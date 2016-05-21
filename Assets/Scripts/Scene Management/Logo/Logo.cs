using System.Collections;
using UnityEngine;

public class Logo : MonoBehaviour
{
    public Animator logoAnimator;

    private void Awake()
    {
        StartCoroutine(LogoProcess());
    }

    private IEnumerator LogoProcess()
    {
        logoAnimator.enabled = true;
        yield return new WaitForSeconds(2f);

        StartCoroutine(SceneFader.Instance.FadeOut(0.6f, "Title"));
    }
}