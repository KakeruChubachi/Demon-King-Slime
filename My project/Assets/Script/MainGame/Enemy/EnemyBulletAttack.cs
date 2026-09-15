using UnityEngine;

public class EnemyBulletAttack : MonoBehaviour
{
    public GameObject Bullet;
    public Transform target;
    private GameObject currentBullet;
    public float attackCooldown = 10.0f;// 攻撃のクールダウン時間
    float AttackTime = 0f;// 最後に攻撃した時間
    public float keepDistance = 5f;   // 保ちたい距離
    public float moveSpeed = 2f;      // 距離調整の移動速度
    public float distanceTolerance = 0.5f; // 許容誤差(近づいたり離れたりの往復を防ぐ)

    void Start()
    {
       target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time - AttackTime >= attackCooldown)
        {
            if (currentBullet != null)
            {
                Destroy(currentBullet);
            }
            currentBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            currentBullet.GetComponent<EnemyBullet>().SetDirection(target);
            AttackTime = Time.time; // 攻撃した時間を更新
        }

        float dist = Vector2.Distance(transform.position, target.position);
        Vector2 dir = (transform.position - target.position).normalized;

        if (dist < keepDistance - distanceTolerance)
        {
            transform.position += (Vector3)dir * moveSpeed * Time.deltaTime; // 遠ざかる
        }
        else if (dist > keepDistance + distanceTolerance)
        {
            transform.position -= (Vector3)dir * moveSpeed * Time.deltaTime; // 近づく
        }
    }
}
