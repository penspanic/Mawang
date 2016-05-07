using UnityEngine;
using System.Collections;
using System;

public class Werewolf : Warrior, ITouchable
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpDis;

    private Transform enemyCastlePos;
    private bool isEndPos = false;
    private float fixedX = 0.0f;
        
    protected override void Awake()
    {
        base.Awake();
        enemyCastlePos = battleMgr.enemyCastle.transform;
    }
    // Mathf.Abs(2 *  sin(x))
    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            StartCoroutine(WarewolfSkill());
        }
    }

    // skill
    IEnumerator WarewolfSkill()
    {
        float currTime = 0.0f;
        Vector3 orginPos = transform.position;
        Vector3 jumpVec = Vector3.zero;
        while(currTime < jumpTime)
        {
            currTime += Time.deltaTime;

            float jumpX = (currTime / jumpTime) * (jumpDis * 3);
            // 만약 x가 적 성을 넘어가면 못넘어가게

            if (!isEndPos)
                jumpVec.x = jumpX;
            else
                jumpVec.x = fixedX;

            jumpVec.y = Mathf.Abs(jumpHeight * Mathf.Sin(jumpX / jumpDis));

            if (transform.position.x >= enemyCastlePos.position.x && !isEndPos)
            {
                fixedX = jumpVec.x;
                isEndPos = true;
            }
            transform.position = orginPos + jumpVec;
            yield return null;
        }

        if (isEndPos)
            fixedX = 0.0f;

        transform.position = new Vector2(transform.position.x, orginPos.y);

    }

    // Animation Event
    public void OnSkillMotionEvent()
    {
        SkillMotionEnd();
    }
}
