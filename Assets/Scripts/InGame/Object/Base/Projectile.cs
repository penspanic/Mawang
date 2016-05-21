using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float pjtileSpeed;

    [SerializeField]
    private Vector2 adjustPos;

    [SerializeField]
    private bool hitMultipleObject;

    [SerializeField]
    private int moveDistance;

    [SerializeField]
    private int canHitNum;

    private BattleManager battleMgr;
    private List<ObjectBase> hitObjectList = new List<ObjectBase>();
    private Movable parent;
    private bool isMoveRight;
    private int damage;
    private ObjectBase target;

    private void Awake()
    {
        battleMgr = GameObject.FindObjectOfType<BattleManager>();
        parent = transform.parent.GetComponent<Movable>();

        damage = parent.GetAttackDamage();
        isMoveRight = parent.isOurForce ? true : false;
    }

    private void OnEnable() // 유닛이 켜질떄
    {
        target = null;

        if (parent.GetComponent<Launcher>())
            target = parent.GetComponent<Launcher>().GetLauncherTargets(0);
        else
        {
            if (parent.GetTargets() != null)
                target = parent.GetTargets()[0];
        }

        if (hitMultipleObject)
        {
            StartCoroutine(Penetrate());
            return;
        }

        if (target != null)
        {
            transform.position = new Vector2(parent.transform.position.x + adjustPos.x,
            parent.transform.position.y + adjustPos.y);

            StartCoroutine(ChaseUnit());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator ChaseUnit()
    {
        while (gameObject.activeSelf == true)
        {
            transform.Translate(Vector2.right * Time.deltaTime * pjtileSpeed * (isMoveRight ? 1 : -1), Space.World);

            if (target != null)
            {
                if (Mathf.Abs(transform.position.x - target.transform.position.x) <= 0.1f) // 거리가 0.1 이하일때
                {
                    target.Attacked(damage);
                    gameObject.SetActive(false);
                }
            }
            else
                gameObject.SetActive(false);

            yield return null;
        }
    }

    private IEnumerator Penetrate() // 목적지까지 가면서 퀘뚫음, 일정 타겟이상 맞췄을 시 종료
    {
        hitObjectList.Clear();
        ObjectBase[] targets = battleMgr.GetTargets(parent, moveDistance, canHitNum);

        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].Attacked(damage);
        }

        Vector2 startPos = parent.transform.position;
        Vector2 endPos = startPos;
        endPos.x += moveDistance * BattleManager.fightDistance;

        transform.position = startPos;
        while (gameObject.activeSelf == true)
        {
            transform.Translate(Vector2.right * Time.deltaTime * pjtileSpeed * (isMoveRight ? 1 : -1), Space.World);

            if (Mathf.Abs(transform.position.x - endPos.x) < 0.1f)
                break;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}