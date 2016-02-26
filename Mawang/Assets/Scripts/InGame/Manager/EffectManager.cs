using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectKind
{
    Dulahan_skill,
    Grim_attack,
    Orc_skill,
    Harpy_skill,
    Skeleton_skill
}

/// <summary>
/// TODO : 자신이 갖고있는 유닛만 오브젝트풀 생성하기
/// </summary>
/// 
// 0.9 -0.16
public class EffectManager : Singleton<EffectManager>
{

    private GameObject grimAttck;
    private GameObject dullahanSkill;
    private GameObject orcSkill;
    private GameObject harpySkill;
    private GameObject skeletonSkill;

    private Dictionary<EffectKind,GameObjectPool<GameObject>> skillDic;

    void Awake()
    {
        skillDic = new Dictionary<EffectKind, GameObjectPool<GameObject>>();

        grimAttck       =   Resources.Load<GameObject>("Prefabs/Effect/Grim_Attack");
        dullahanSkill   =   Resources.Load<GameObject>("Prefabs/Effect/Dullahan_Skill");
        orcSkill        =   Resources.Load<GameObject>("Prefabs/Effect/Orc_Skill");
        harpySkill      =   Resources.Load<GameObject>("Prefabs/Effect/Harpy_Skill");
        skeletonSkill   =   Resources.Load<GameObject>("Prefabs/Effect/Skeleton_Skill");

        skillDic.Add(EffectKind.Grim_attack, new GameObjectPool<GameObject>(3, grimAttck, SetObject));
        skillDic.Add(EffectKind.Dulahan_skill, new GameObjectPool<GameObject>(3, dullahanSkill, SetObject));
        skillDic.Add(EffectKind.Orc_skill, new GameObjectPool<GameObject>(3, orcSkill, SetObject));
        skillDic.Add(EffectKind.Harpy_skill, new GameObjectPool<GameObject>(3, harpySkill, SetObject));
        skillDic.Add(EffectKind.Skeleton_skill, new GameObjectPool<GameObject>(3, skeletonSkill, SetObject));
    }

    GameObject SetObject(GameObject obj)
    {
        GameObject clone = GameObject.Instantiate(obj);
        clone.SetActive(false);
        return clone;
    }


    public void PlayEffect(EffectKind ek, Vector2 spawnPos,Transform SetParent = null)
    {

        GameObject obj = skillDic[ek].pop();

        obj.GetComponent<SpriteRenderer>().color = Color.white;
        obj.transform.position = spawnPos;

        if (SetParent != null)
            obj.transform.SetParent(SetParent);

        obj.SetActive(true);

        obj.GetComponent<SpriteDelayedDisappear>().callBack = (GameObject old) =>
            {
                if (SetParent != null)
                    old.transform.SetParent(null);

                old.SetActive(false);
                skillDic[ek].push(old);
            };
    }
}
