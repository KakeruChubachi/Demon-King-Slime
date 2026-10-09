using UnityEngine;

public class FanAttacker : MonoBehaviour
{
    GameObject swingPrefab;
    float damage, interval, range, angle, timer;
    Player player;

    public void Setup(GameObject swingPrefab, float damage, float interval, float range, float angle)
    {
        this.swingPrefab = swingPrefab;
        this.damage = damage;
        this.interval = interval;
        this.range = range;
        this.angle = angle;
    }

    void Start()
    {
        player = GetComponent<Player>();
        Attack();
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
        Vector2 facing = player.FacingDirection;
        float facingAngle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;

        if (swingPrefab != null)
        {
            var fx = Instantiate(swingPrefab, transform.position, Quaternion.Euler(0, 0, facingAngle));
            Destroy(fx, 0.6f);
        }

        foreach (var col in Physics2D.OverlapCircleAll(transform.position, range, player.enemyLayer))
        {
            Vector2 toEnemy = col.transform.position - transform.position;
            if (Vector2.Angle(facing, toEnemy) > angle * 0.5f) continue;
            col.GetComponent<Enemy>()?.TakeDamage(Mathf.RoundToInt(damage), Enemy.AttackType.Physical);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 facing = Vector2.right;
        var p = GetComponent<Player>();
        if (p != null) facing = p.FacingDirection;

        Gizmos.color = Color.yellow;
        float a = angle * 0.5f;
        Vector3 l = Quaternion.Euler(0, 0, a) * facing * range;
        Vector3 r = Quaternion.Euler(0, 0, -a) * facing * range;
        Gizmos.DrawLine(transform.position, transform.position + l);
        Gizmos.DrawLine(transform.position, transform.position + r);
    }
}