using UnityEngine;

public class EnemyBulletAttack : MonoBehaviour
{
    public GameObject Bullet;
    public Transform target;
    private GameObject currentBullet;
    public float attackCooldown = 10.0f;// 攻撃のクールダウン時間
    float AttackTime = 0f;// 最後に攻撃した時間

    void Start()
    {

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
    }
}
