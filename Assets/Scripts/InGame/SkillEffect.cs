using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public void OnEffectEnd()
    {
        gameObject.SetActive(false);
    }
}