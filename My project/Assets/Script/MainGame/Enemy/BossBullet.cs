using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damage = 3;
    public float lifeTime = 6f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}