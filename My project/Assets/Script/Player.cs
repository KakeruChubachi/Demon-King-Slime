using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float playerRadius = 1.0f;
    public float attackRangeMultiplier = 1.5f;//攻撃範囲
    public float attackCooldown = 1.0f; // 攻撃のクールダウン時間
    public int hp = 10; // プレイヤーの体力
    public int exp = 1;//経験値
    public float damageCooldown = 1.0f; // ダメージを受けた後の無敵時間
    float lastDamageTime = -999;

    float lastAttackTime = 0f; // 最後に攻撃した時間

    // Update is called once per frame
    void Update()
    {
        //入力を調べる
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        //移動ベクトルを作る
        Vector2 moveDirection = new Vector2(inputX, inputY);

        //実際の移動量
        Vector2 movement = moveDirection * moveSpeed * Time.deltaTime;

        //位置を更新
        transform.position += new Vector3(movement.x, movement.y, 0);

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time; // 攻撃した時間を更新
            }
        }
        */

        AutoAttack();
    }

    void Attack()
    {
        //攻撃範囲
        float attackRadius = playerRadius * attackRangeMultiplier;

        //attackRadiusの範囲内にいる敵を取得
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        //取得した敵の数だけ、仮の確認ログを出す
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("敵を攻撃しました: " + enemy.name);
            Enemy ediscovery = enemy.GetComponent<Enemy>();
            if(ediscovery != null )
            {
                ediscovery.TakeDamage(1);
            }

        }
    }

    
     void AutoAttack()
     {
        if(Time.time - lastAttackTime >= attackCooldown)
        {  
            Attack();
            lastAttackTime = Time.time;
        }
     }
     

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float attackRadius = playerRadius * attackRangeMultiplier;

        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    // 敵(トリガー)に触れた瞬間、自動で呼ばれる関数
    void OnTriggerStay2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        // TODO: other から Enemy スクリプトを取り出す(GetComponentを使う)
        Enemy enemy = other.GetComponent<Enemy>();// ヒント: other.GetComponent<Enemy>()

        // TODO: enemyがnullでない(=本当にEnemyだった)場合だけダメージ処理を行う
        if (enemy != null)// ヒント: enemy != null という条件
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage(1); // 仮のダメージ量
                lastDamageTime = Time.time; // ダメージを受けた時間を更新
            }
        }

        ExpOrb expOrb = other.GetComponent<ExpOrb>();
        if(expOrb != null)
        {
            exp += expOrb.PickupExp();
            Debug.Log("現在の経験値："+ exp);
        }
    }

    // Player自身がダメージを受ける関数(Enemy.csのTakeDamageと同じ考え方)
    public void TakeDamage(int damage)
    {
        // TODO: hp から damage を引く
        hp -= damage;

        // TODO: hp が 0 以下になったかどうかを調べる
        if (hp <= 0)// TODO: 条件
    {
            // 今はひとまずログを出すだけにしておく
            Debug.Log("ゲームオーバー");
            FindFirstObjectByType<SceneFader>().FadeToScene("Result");
        }
    }
}
