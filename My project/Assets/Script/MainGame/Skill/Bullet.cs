using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private Vector2 direction;

    public void SetDirection(Vector2 targetDirection)
    {
        direction = targetDirection.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }
}