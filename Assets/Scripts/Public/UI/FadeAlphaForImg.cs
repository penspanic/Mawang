using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeAlphaForImg : MonoBehaviour
{
    [Range(0,1)]
    public float minAlpha = 0;
    [Range(0,1)]
    public float maxAlpha = 1;

    public float fadingTime;

    private Image targetImg;
    private bool isUpping;
    void Awake()
    {
        targetImg = gameObject.GetComponent<Image>();
        isUpping  = false;
    }


    void OnEnable()
    {
        StartCoroutine(FadeAlpha());
    }

    IEnumerator FadeAlpha()
    {
        while (true)
        {
            float currTime = 0.0f;

            if(isUpping)
                targetImg.color = SetAlpha(targetImg, minAlpha);
            else
                targetImg.color = SetAlpha(targetImg, maxAlpha);

            while (currTime < fadingTime)
            {
                currTime += EasingUtil.tick;

                float al;
                if (isUpping)
                    al = EasingUtil.easeOutCubic(minAlpha, maxAlpha, currTime / fadingTime);
                else
                    al = EasingUtil.easeInCubic(maxAlpha, minAlpha, currTime / fadingTime);

                targetImg.color = SetAlpha(targetImg, al);

                yield return null;
            }

            if (isUpping)
                targetImg.color = SetAlpha(targetImg, maxAlpha);
            else
                targetImg.color = SetAlpha(targetImg, minAlpha);

            isUpping = !isUpping;
        }
    }

    Color SetAlpha(Image img, float alpha)
    {
        return new Color(img.color.r, img.color.g, img.color.b, alpha);
    }
}
