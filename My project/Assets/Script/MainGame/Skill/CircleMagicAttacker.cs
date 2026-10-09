using UnityEngine;

public class CircleMagicAttacker : MonoBehaviour
{
    GameObject magicPrefab;
    float damage, interval, radius, timer;
    Player player;

    public void Setup(GameObject magicPrefab, float damage, float interval, float radius)
    {
        this.magicPrefab = magicPrefab;
        this.damage = damage;
        this.interval = interval;
        this.radius = radius;
    }

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer < interval) return;
        timer = 0f;
        Attack();
    }

    void Attack()
    {
        if (magicPrefab != null)
        {
            var fx = Instantiate(magicPrefab, transform.position, Quaternion.identity);
            fx.transform.localScale = Vector3.one * radius * 2f; // ’¼Œa1ƒ†ƒjƒbƒg‚ÌŒ©‚½–Ú‚ð”ÍˆÍ‚É‡‚í‚¹‚é
            Destroy(fx, 0.5f);
        }

        foreach (var col in Physics2D.OverlapCircleAll(transform.position, radius, player.enemyLayer))
        {
            col.GetComponent<Enemy>()?.TakeDamage(Mathf.RoundToInt(damage), Enemy.AttackType.Magic);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}