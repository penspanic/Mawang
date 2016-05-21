using UnityEngine;

public class SatanSkill : MonoBehaviour
{
    private ObjectBase target;

    [SerializeField]
    private float distanceY;     // Y 보정

    public int attackDamage
    {
        get;
        set;
    }

    private void Awake()
    {
        // 위치 보정
        this.transform.position = new Vector2(transform.position.x, transform.position.y + distanceY);
    }

    private void Attack()
    {
        target.Attacked(attackDamage);
    }

    // 타겟 설정해줌
    public void SetTarget(ObjectBase obj)
    {
        if (obj == null)
        {
            Destroy(gameObject);
            return;
        }

        target = obj;
        Attack();
    }

    // 애니메이션 끝날때 이 함수 호출함
    public void OnAttackEnd()
    {
        Destroy(gameObject);
    }
}