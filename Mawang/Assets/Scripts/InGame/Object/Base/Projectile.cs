using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float       pjtileSpeed;
    [SerializeField]
    private Vector2     adjustPos;

    private Launcher    parent;
    private bool        isMoveRight;
    private int         damage;
    private ObjectBase  target;

    void Awake()
    {
        parent = transform.parent.GetComponent<Launcher>();

        damage = parent.GetAttackDamage();
        isMoveRight = parent.isOurForce ? true : false;
    }

    void OnEnable() // 유닛이 켜질떄
    {
        target = parent.GetLauncherTargets(0);

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



}
