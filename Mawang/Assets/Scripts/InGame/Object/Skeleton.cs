using UnityEngine;
using System.Collections;

public class Skeleton : Warrior,ITouchable
{
    [SerializeField]
    private int hpCost;
    [SerializeField]
    private int growthDmg;
    [SerializeField]
    private float duration;

    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {

            Vector2 spawnPos = transform.position;
            spawnPos += new Vector2(-0.1f, 0.7f);
            EffectManager.Instance.PlayEffect(EffectKind.Skeleton_skill, spawnPos, transform);

            StartCoroutine(SkeletonSkill());

        }
    }

    IEnumerator SkeletonSkill()
    {
        if (hp - hpCost <= 0)
            hp = 1;
        else
            hp -= hpCost;


        attackDamage += growthDmg;
        yield return new WaitForSeconds(duration);
        attackDamage -= growthDmg;


        yield break;
    }
}
