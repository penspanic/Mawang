using UnityEngine;
using System.Collections;

public class Grim : Warrior, ITouchable
{

    protected override void Awake()
    {
        base.Awake();
        if (forDecoration)
            return;
        canUseSkill = true;
    }


    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            // skillEffect.SetActive(true);
            // StartCoroutine(DullahanSkill());
        }
    }

    public void OnSkillMotionEvent()
    {
        // skillEffect.SetActive(false);
        SkillMotionEnd();
    }
}
