using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Witch : Launcher, ITouchable
{
    [SerializeField]
    private int skillDuration;

    [SerializeField]
    private float healAmount;
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private int skillRange;

    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            EffectManager.instance.PlayEffect(EffectKind.Witch_skill, transform.position + new Vector3(-1f, 1f, 0f));
            EffectManager.instance.PlayEffect(EffectKind.Witch_skill, transform.position + new Vector3(1.6f, 1f, 0f));
            StartCoroutine(SkillProcess());
        }
    }

    IEnumerator SkillProcess()
    {
        float elapsedTIme = 0f;
        Movable[] targets = null;

        while (elapsedTIme < skillDuration)
        {
            elapsedTIme += 1f;

            targets = battleMgr.GetAllUnitInLine(line);
            
            for (int i = 0; i < targets.Length; ++i)
            {
                if (!IsInSkillRange(targets[i]))
                    continue;

                if(targets[i].isOurForce)
                { // 아군 힐
                    int newHP = targets[i].GetHP() + (int)(healAmount / skillDuration);
                    targets[i].SetHP(newHP);
                }
                else
                { // 적군 독데미지
                    int newHP = targets[i].GetHP() - (int)(damageAmount / skillDuration);
                    targets[i].SetHP(newHP);
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    bool IsInSkillRange(Movable target)
    {
        if (Mathf.Abs(this.transform.position.x - target.transform.position.x) < skillRange * BattleManager.tileSize)
            return true;
        else
            return false;
    }
}