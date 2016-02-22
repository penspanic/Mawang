using UnityEngine;
using System.Collections;

public class SkillEffect : MonoBehaviour 
{

    public void OnEffectEnd()
    {
        gameObject.SetActive(false);
    }
}
