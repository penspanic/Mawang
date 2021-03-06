﻿using System.Collections.Generic;
using UnityEngine;

public enum EffectKind
{
    Dulahan_skill,
    Grim_attack,
    Orc_skill,
    Harpy_skill,
    Skeleton_skill,
    Bandsman_skill,
    Witch_skill,
    Werewolf_Attack,
    Werewolf_Skill,
    Doll_SKill,
    Aragog_Effect
}

/// <summary>
/// TODO : 자신이 갖고있는 유닛만 오브젝트풀 생성하기
/// </summary>
///
public class EffectManager : Singleton<EffectManager>
{
    private GameObject grimAttck;
    private GameObject dullahanSkill;
    private GameObject orcSkill;
    private GameObject harpySkill;
    private GameObject skeletonSkill;
    private GameObject bandsmanSkill;
    private GameObject witchSkill;
    private GameObject werewolfAttack;
    private GameObject werewolfSkill;
    private GameObject dollSkill;
    private GameObject aragogEffect;

    private GameObject harpyBuff;

    private Dictionary<EffectKind, ObjectPool<GameObject>> skillDic;

    private void Awake()
    {
        skillDic = new Dictionary<EffectKind, ObjectPool<GameObject>>();

        grimAttck = Resources.Load<GameObject>("Prefabs/Effect/Grim_Attack");
        dullahanSkill = Resources.Load<GameObject>("Prefabs/Effect/Dullahan_Skill");
        orcSkill = Resources.Load<GameObject>("Prefabs/Effect/Orc_Skill");
        harpySkill = Resources.Load<GameObject>("Prefabs/Effect/Harpy_Skill");
        skeletonSkill = Resources.Load<GameObject>("Prefabs/Effect/Skeleton_Skill");
        bandsmanSkill = Resources.Load<GameObject>("Prefabs/Effect/Bandsman_Skill");
        witchSkill = Resources.Load<GameObject>("Prefabs/Effect/Witch_Skill");
        werewolfAttack = Resources.Load<GameObject>("Prefabs/Effect/Werewolf_Attack");
        werewolfSkill = Resources.Load<GameObject>("Prefabs/Effect/Werewolf_Skill");
        dollSkill = Resources.Load<GameObject>("Prefabs/Effect/Doll_Skill");
        aragogEffect = Resources.Load<GameObject>("Prefabs/Effect/Aragog_Effect");

        skillDic.Add(EffectKind.Grim_attack, new ObjectPool<GameObject>(3, grimAttck, SetObject));
        skillDic.Add(EffectKind.Dulahan_skill, new ObjectPool<GameObject>(3, dullahanSkill, SetObject));
        skillDic.Add(EffectKind.Orc_skill, new ObjectPool<GameObject>(3, orcSkill, SetObject));
        skillDic.Add(EffectKind.Harpy_skill, new ObjectPool<GameObject>(3, harpySkill, SetObject));
        skillDic.Add(EffectKind.Skeleton_skill, new ObjectPool<GameObject>(3, skeletonSkill, SetObject));
        skillDic.Add(EffectKind.Bandsman_skill, new ObjectPool<GameObject>(3, bandsmanSkill, SetObject));
        skillDic.Add(EffectKind.Witch_skill, new ObjectPool<GameObject>(3, witchSkill, SetObject));
        skillDic.Add(EffectKind.Werewolf_Attack, new ObjectPool<GameObject>(3, werewolfAttack, SetObject));
        skillDic.Add(EffectKind.Werewolf_Skill, new ObjectPool<GameObject>(3, werewolfSkill, SetObject));
        skillDic.Add(EffectKind.Doll_SKill, new ObjectPool<GameObject>(3, dollSkill, SetObject));
        skillDic.Add(EffectKind.Aragog_Effect, new ObjectPool<GameObject>(3, aragogEffect, SetObject));
    }

    private GameObject SetObject(GameObject obj)
    {
        GameObject clone = GameObject.Instantiate(obj);
        clone.SetActive(false);
        clone.transform.SetParent(this.transform);
        return clone;
    }

    public void PlayEffect(EffectKind ek, Vector2 spawnPos, Transform parent = null)
    {
        GameObject obj = skillDic[ek].pop();

        obj.GetComponent<SpriteRenderer>().color = Color.white;
        obj.transform.position = spawnPos;

        if (parent != null)
            obj.transform.SetParent(parent);

        obj.SetActive(true);

        obj.GetComponent<SpriteDelayedDisappear>().callBack = (GameObject old) =>
            {
                if (parent != null)
                    old.transform.SetParent(null);

                old.SetActive(false);
                skillDic[ek].push(old);
            };
    }
}