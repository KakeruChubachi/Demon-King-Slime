using UnityEngine;

public class FireShooter : MonoBehaviour
{
    FireProjectile prefab;
    float damage, interval, range, timer;

    public void Setup(FireProjectile prefab, float damage, float interval, float range)
    {
        this.prefab = prefab;
        this.damage = damage;
        this.interval = interval;
        this.range = range;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < interval) return;

        Transform target = FindNearestEnemy();
        if (target == null) return;   // “G‚ª‚¢‚È‚¯‚ê‚Î‘Ò‹@

        timer = 0f;
        Vector2 dir = (target.position - transform.position).normalized;
        var p = Instantiate(prefab, transform.position, Quaternion.identity);
        p.Launch(dir, damage);
    }

    Transform FindNearestEnemy()
    {
        Transform best = null;
        float bestSqr = range * range;
        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float d = ((Vector2)(e.transform.position - transform.position)).sqrMagnitude;
            if (d < bestSqr) { bestSqr = d; best = e.transform; }
        }
        return best;
    }
}