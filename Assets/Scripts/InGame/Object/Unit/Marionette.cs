using UnityEngine;
using System.Collections;
using System;

public class Marionette : Movable, ITouchable
{
    [SerializeField]
    Movable dollPrefab;
    [SerializeField]
    float spawnCoolTime;

    bool canSpawn;
    bool dollSpawned;
    Doll currDoll;


    protected override void Awake()
    {
        base.Awake();
        canSpawn = true;
    }
    protected override void Attack()
    {
        base.Attack();
        if(!dollSpawned && canSpawn)
            SpawnDoll();
    }
    
    void SpawnDoll()
    {
        currDoll = spawnMgr.SpawnOurForce(dollPrefab, line, transform.position) as Doll;
        currDoll.Init(this);
        dollSpawned = true;
        canSpawn = false;
    }

    public void OnDollDeath()
    {
        dollSpawned = false;
        currDoll = null;
        StartCoroutine(SpawnWaiting());
    }

    public void OnTouch()
    {
        if(state == MovableState.Attack && canUseSkill && !isDestroyed
            && currDoll != null && !currDoll.isDestroyed)
        {
            animator.Play("Skill", 0);
            SkillMotionStart();
            currDoll.OnSkill();
        }
    }

    IEnumerator SpawnWaiting()
    {
        yield return new WaitForSeconds(spawnCoolTime);
        canSpawn = true;
    }
}