using UnityEngine;
using System.Collections;

public class Boss3 : Enemy
{
    public float tackleSpeed = 8f;//タックルの速度
    public float tackleDistance = 10f;//タックルの距離
    public GameObject dashWarning; // ゲームオブジェクトの参照
    public GameObject dashWarningVisual; // 警告のビジュアルの参照
    float dashWarningMultiplier = 0.2f; // 警告の幅をタックル距離に設定するための倍率

    public GameObject bulletPrefab; // 弾のプレハブ
    public float bulletSpeed = 5f; // 弾の速度
    public float fireRate = 1f; // 弾の発射間隔
    public float bulletAngle = 0f; // 弾の角度
    public int bulletType = 0; // 弾の種類（0:通常弾、1:追尾弾、2:拡散弾など）
    public int bulletTypeChangeTime = 10; // 弾の種類を変更する時間（秒）
    public Player player; // Transform型ではなくPlayer型
    public bool isDashing = false; // タックル中かどうかを示すフラグ
    protected override void Start()
    {
        base.Start();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        StartCoroutine(FireBulletsCoroutine());
        StartCoroutine(BossMovesCoroutine());
    }

    public IEnumerator BossMovesCoroutine()
    {
        
        while (true)
        {
            movementEnabled = true; // 移動を有効にする
            yield return new WaitForSeconds(1f); // 1秒間移動
            movementEnabled = false; // 移動を無効にする
            Vector3 direction = target.position - transform.position;
            direction = direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            dashWarning.transform.eulerAngles = new Vector3(0f, 0f, angle);
            Vector3 newScale = dashWarningVisual.transform.localScale;
            newScale.x = tackleDistance * dashWarningMultiplier; // 警告の幅をタックル距離に設定
            dashWarningVisual.transform.localScale = newScale;
            dashWarning.SetActive(true); // 警告を表示
            yield return new WaitForSeconds(0.5f); // 0.5秒間停止


            float distanceTraveled = 0f; // タックルの移動距離を保存する変数
            while (distanceTraveled < tackleDistance)
            {
                isDashing = true; // タックル中であることを示すフラグを立てる
                float Distancecovered = tackleSpeed * Time.deltaTime; // タックルの移動距離を計算
                transform.position += direction * Distancecovered; // タックルの移動
                distanceTraveled += Distancecovered; // 移動距離を更新
                yield return null; // 次のフレームまで待機
            }
            isDashing = false; // タックルが終了したことを示すフラグを下ろす
            dashWarning.SetActive(false); // 警告を非表示
        }
        
    }

    public IEnumerator FireBulletsCoroutine()
    {
        while (true)
        {
            //if (!isDashing)
            //{
            //    yield return null;
            //    continue;
            //}

            // 拡散弾の処理(360度全方位)
            int bulletCount = 12;
            float angleStep = 360f / bulletCount;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = angleStep * i;
                GameObject spreadBullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));
                Rigidbody2D spreadRb = spreadBullet.GetComponent<Rigidbody2D>();
                spreadRb.linearVelocity = spreadBullet.transform.right * bulletSpeed;
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}
