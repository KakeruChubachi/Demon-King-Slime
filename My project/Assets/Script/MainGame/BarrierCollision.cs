using UnityEngine;

public class Barrier : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("Enemy‚ªBarrier‚ÉG‚ê‚½");
            enemy.movementEnabled = false; // Enemy‚ÌˆÚ“®‚ğ–³Œø‰»
        }

        EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            Destroy(enemyBullet.gameObject); // Enemybullet‚ğ”j‰ó
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.movementEnabled = true; // Enemy‚ÌˆÚ“®‚ğ—LŒø‰»
        }
    }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
