using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float DashSpeed = 10f;
    public bool isDashing = false;
    public float playerRadius = 1.0f;
    public float attackRangeMultiplier = 1.5f;//攻撃範囲
    public float attackCooldown = 1.0f; // 攻撃のクールダウン時間
    public int hp = 10; // プレイヤーの体力

    public int exp = 1;//経験値
    public int nowLevel = 1;//現在のレベル
    public int levelUpExp = 10;//レベルアップに必要な経験値
    public UIController uIController;
    public List<SkillOrb> nearSkillOrbs = new List<SkillOrb>();
    SkillData skillData;

    public float damageCooldown = 1.0f; // ダメージを受けた後の無敵時間
    public float dashDuration = 0.15f; // ダッシュの持続時間
    float lastDamageTime = -999;
    float lastAttackTime = 0f; // 最後に攻撃した時間
    public LayerMask enemyLayer; // 敵のレイヤーを指定するための変数
    public bool invinCible = false; // 無敵状態かどうかを示すフラグ
    public float invincibleDuration = 2.0f; // 無敵状態の持続時間
    public GameObject barrierVisual;
    public SpriteRenderer spriteRenderer; // プレイヤーのスプライトレンダラーを参照するための変数
    public float copyDuration = 5.0f; // コピーの持続時間

    //元のステータスを保存する変数
    int originalHp;
    float originalMoveSpeed;
    Sprite originalSprite;

    void Start()
    {
        uIController.SetSllimeLevel(nowLevel);
        uIController.SetLife(hp);
        barrierVisual.SetActive(false); // バリア状態のビジュアルを非表示にする
    }

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

        if (isDashing) return; // ダッシュ中は通常の移動を無効化

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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        //取得した敵の数だけ、仮の確認ログを出す
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("敵を攻撃しました: " + enemy.name);
            Enemy ediscovery = enemy.GetComponent<Enemy>();
            if (ediscovery != null)
            {
                ediscovery.TakeDamage(1);
            }

        }
    }


    void AutoAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void LevelUp()
    {
        if (exp >= levelUpExp)
        {
            nowLevel++;
            exp -= levelUpExp;
            levelUpExp += 10; // 次のレベルアップに必要な経験値を増やす
            uIController.SetSllimeLevel(nowLevel);
            Debug.Log("レベルアップ！現在のレベル：" + nowLevel);
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

        EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                TakeDamage(1);
                lastDamageTime = Time.time;
            }
        }

        ExpOrb expOrb = other.GetComponent<ExpOrb>();
        if (expOrb != null)
        {
            exp += expOrb.PickupExp();
            Debug.Log("現在の経験値：" + exp);
            LevelUp();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            nearSkillOrbs.Add(nearSkillOrb);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            nearSkillOrbs.Remove(nearSkillOrb) ;
        }
    }

    // Player自身がダメージを受ける関数(Enemy.csのTakeDamageと同じ考え方)
    public void TakeDamage(int damage)
    {
        if (invinCible)
        {
            return; // 無敵状態ならダメージを受けない
        }
        // TODO: hp から damage を引く
        hp -= damage;
        uIController.SetLife(hp);

        // TODO: hp が 0 以下になったかどうかを調べる
        if (hp <= 0)// TODO: 条件
        {
            // 今はひとまずログを出すだけにしておく
            Debug.Log("ゲームオーバー");
            FindFirstObjectByType<SceneFader>().FadeToScene("Result");
        }
    }

    public IEnumerator InvincibleCoroutine()
    {
        invinCible = true; // 無敵状態にする
        barrierVisual.SetActive(true); // バリア状態のビジュアルを表示する
        yield return new WaitForSeconds(invincibleDuration); // 無敵状態の持続時間を待つ
        invinCible = false; // 無敵状態を解除する
        barrierVisual.SetActive(false); // バリア状態のビジュアルを非表示にする
    }

    public void ActivateBarrier()
    {
        StartCoroutine(InvincibleCoroutine());
    }

    public void ActivateCopy()
    {
        if (nearSkillOrbs.Count > 0)
        {
            SkillOrb orbTouse = nearSkillOrbs[0];
            skillData = orbTouse.GetSkillOrb();
            nearSkillOrbs.Remove(orbTouse);
        }
        if (skillData == null)
        {
            return;
        }
        StartCoroutine(CopyCoroutine());
        // コピーの効果を発動する処理をここに追加
        Debug.Log("コピーの効果を発動しました！");
    }

    public void ActivateAvoidance()
    {
        StartCoroutine(AvoidanceCoroutine());
        // 回避の効果を発動する処理をここに追加
        Debug.Log("回避の効果を発動しました！");
    }

    public IEnumerator AvoidanceCoroutine()
    {
        //入力を調べる
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        //移動ベクトルを作る
        Vector2 moveDirection = new Vector2(inputX, inputY);

        isDashing = true; // ダッシュ状態にする

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position += new Vector3(moveDirection.x,moveDirection.y,0) * DashSpeed * Time.deltaTime;
            yield return null; // 次のフレームまで待つ
        }

        isDashing = false; // ダッシュ状態を解除する
    }

    public IEnumerator CopyCoroutine()
    {
        //元のステータスを保存
        originalHp = hp;
        originalMoveSpeed = moveSpeed;
        originalSprite = spriteRenderer.sprite;

        // コピーされたスキルデータのステータスを適用
        hp = skillData.copiedHp;
        moveSpeed = skillData.copiedMoveSpeed;
        spriteRenderer.sprite = skillData.copiedSprite;

        yield return new WaitForSeconds(copyDuration); // コピーの効果が持続する時間

        // 元のステータスに戻す
        hp = originalHp;
        moveSpeed = originalMoveSpeed;
        spriteRenderer.sprite = originalSprite;

        skillData = null; // コピーされたスキルデータをリセット
    }
}