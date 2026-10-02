using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;

    private Vector3 direction;

    public void SetDirection(Transform target)
    {
        direction = (target.position - transform.position).normalized;//Player‚Ì•ûŒü
    }

    void Start()
    {
        Destroy(gameObject, 8f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}