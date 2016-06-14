using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpy : Launcher, ITouchable
{
    [SerializeField]
    private float buffDuration;

    [SerializeField]
    private int growthDmg;

    [SerializeField]
    private GameObject buff_AS;

    private List<ObjectBase> lineList = new List<ObjectBase>();

    public void OnTouch()
    {
        if (canUseSkill && !isDestroyed)
        {
            SkillMotionStart();
            Vector2 spawnPos = transform.position;
            spawnPos += new Vector2(0.9f, -0.16f);
            EffectManager.instance.PlayEffect(EffectKind.Harpy_skill, spawnPos);
            StartCoroutine(BuffRoutine());
        }
    }

    private IEnumerator BuffRoutine()
    {
        lineList = battleMgr.ourForceList.FindAll(e => e.line == line &&
            this.attackRange * BattleManager.tileSize > Mathf.Abs(transform.position.x - e.transform.position.x));

        lineList.Remove(this);
        AddBuffsprRenderer();
        BuffSet(true);
        yield return new WaitForSeconds(buffDuration);
        BuffSet(false);
    }

    private void AddBuffsprRenderer()
    {
        for (int i = 0; i < lineList.Count; ++i)
        {
            if (lineList[i].transform.FindChild("Harpy_buff(Clone)") != null)
            {
                GameObject tmp = lineList[i].transform.FindChild("Harpy_buff(Clone)").gameObject;
                tmp.GetComponent<SpriteRenderer>().color = Color.white;
                tmp.SetActive(true);
                continue;
            }

            GameObject go = Instantiate(buff_AS);
            go.SetActive(true);

            go.transform.SetParent(lineList[i].transform);

            go.transform.localPosition = new Vector2(0, 1.6f);
            AdjustBuffPos(go, lineList[i].name);
        }
    }

    private void BuffSet(bool set)
    {
        for (int i = 0; i < lineList.Count; ++i)
        {
            if (lineList[i] == null)
                continue;

            if (set)
                lineList[i].GetComponent<Movable>().SetAddAttackDmg(growthDmg);
            else
                lineList[i].GetComponent<Movable>().SetAddAttackDmg(-growthDmg);
        }
    }

    public void AdjustBuffPos(GameObject go, string name)
    {
        switch (name)
        {
            case "Goblin(Clone)":
                go.transform.localPosition += new Vector3(0.35f, 0f);
                break;

            case "Orc(Clone)":
                go.transform.localPosition += new Vector3(0.25f, 1.4f);
                break;

            case "Grim(Clone)":
                go.transform.localPosition += new Vector3(0.6f, -0.2f);
                break;

            case "Dullahan(Clone)":
                go.transform.localPosition += new Vector3(-0.5f, -0.6f);
                break;

            case "Harpy(Clone)":
                go.transform.localPosition += new Vector3(1.1f, 0.5f);
                break;
        }
    }

    public void OnSkillMotionEvent()
    {
        SkillMotionEnd();
    }
}