using UnityEngine;
using System.Collections;

public class Boss2 : Enemy
{
    /*弾幕を出すボスに必用な変数
     ・弾の速度
     ・弾の角度
     ・弾の種類
     */
    public GameObject bulletPrefab; // 弾のプレハブ
    public float bulletSpeed = 5f; // 弾の速度
    public float fireRate = 1f; // 弾の発射間隔
    public float bulletAngle = 0f; // 弾の角度
    public int bulletType = 0; // 弾の種類（0:通常弾、1:追尾弾、2:拡散弾など）
    public int bulletTypeChangeTime = 10; // 弾の種類を変更する時間（秒）
    public Player player; // Transform型ではなくPlayer型

    protected override void Start()
    {
       
        base.Start();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        StartCoroutine(FireBulletsCoroutine());
    }


    public IEnumerator FireBulletsCoroutine()
    {
        while (true)
        {
            Vector2 dirToPlayer = (player.transform.position - transform.position).normalized;
            bulletAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
            // 弾の種類に応じた処理
            switch (bulletType)
            {
                case 0:
                case 1:
                    // 通常弾・追尾弾未実装分の処理
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, bulletAngle));
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.linearVelocity = bullet.transform.right * bulletSpeed;
                    rb.angularVelocity = 0f;
                    break;
                case 2:
                    // 拡散弾の処理(360度全方位)
                    int bulletCount = 12; // 全方位に出す弾の数
                    float angleStep = 360f / bulletCount; // 360度をbulletCountで均等分割
                    for (int i = 0; i < bulletCount; i++)
                    {
                        float angle = angleStep * i; // 0度から始めて均等に配置
                        GameObject spreadBullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                        Rigidbody2D spreadRb = spreadBullet.GetComponent<Rigidbody2D>();
                        spreadRb.linearVelocity = spreadBullet.transform.right * bulletSpeed;
                    }
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(fireRate); // 発射間隔待機
                                                       //時間までループを繰り返したらbulletTypeを変更する
            if (Time.time >= bulletTypeChangeTime)
            {
                bulletType = (bulletType + 1) % 3; // 0,1,2の順に変更
                bulletTypeChangeTime += 10; // 次の変更時間を設定
            }
        }
    }
}
