using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    float damage;

    public void Launch(Vector2 dir, float damage)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().linearVelocity = dir * speed; // Unity 2022ˆÈ‘O‚Í velocity
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        other.GetComponent<Enemy>()?.TakeDamage(Mathf.RoundToInt(damage));
        Destroy(gameObject);
    }
}