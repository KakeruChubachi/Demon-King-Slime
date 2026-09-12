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
    public StorenSkillslot storenSkillslot;
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

    // ── 固定マップ範囲 ──
    public Vector2 mapMin = new Vector2(-20f, -20f);
    public Vector2 mapMax = new Vector2(20f, 20f);

    // ── 攻撃タイプ別ステータス ──
    public int physicalPower = 3;  // 近接攻撃力
    public int rangedPower = 2;    // 遠距離攻撃力
    public int magicPower = 5;     // 魔法攻撃力

    public GameObject rangedEffectPrefab; // 遠距離攻撃のエフェクトプレハブ
    public GameObject magicEffectPrefab;  // 魔法攻撃のエフェクトプレハブ
    public float rangedRange = 8f;        // 遠距離攻撃の射程
    public float magicRange = 6f;         // 魔法攻撃の射程
    public float rangedCooldown = 1.5f;   // 遠距離攻撃のクールダウン
    public float magicCooldown = 3.0f;    // 魔法攻撃のクールダウン
    float lastRangedTime = 0f;
    float lastMagicTime = 0f;

    //元のステータスを保存する変数
    int originalHp;
    float originalMoveSpeed;
    Sprite originalSprite;

    public bool canRangedAttack = false; // 遠距離攻撃が使えるかどうか
    public bool canMagicAttack = false; // 魔法攻撃が使えるかどうか

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

        // 固定マップ範囲でClamp（カメラ位置に依存しない）
        Vector3 mapArea = transform.position;
        mapArea.x = Mathf.Clamp(transform.position.x, mapMin.x, mapMax.x);
        mapArea.y = Mathf.Clamp(transform.position.y, mapMin.y, mapMax.y);
        transform.position = mapArea;

        AutoAttack();
    }

    // ── 近接攻撃 ──
    void PhysicalAttack()
    {
        float attackRadius = playerRadius * attackRangeMultiplier;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("物理攻撃がヒット: " + enemy.name);
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(physicalPower);
            }
        }
    }

    // ── 遠距離攻撃 ──
    void RangedAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, rangedRange, enemyLayer);
        if (hitEnemies.Length == 0) return;

        // 一番近い敵を1体だけ狙う
        Collider2D nearest = null;
        float nearestDist = Mathf.Infinity;
        foreach (Collider2D enemy in hitEnemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = enemy;
            }
        }

        if (nearest == null) return;

        Debug.Log("遠距離攻撃がヒット: " + nearest.name);

        if (rangedEffectPrefab != null)
        {
            Instantiate(rangedEffectPrefab, nearest.transform.position, Quaternion.identity);
        }

        Enemy e = nearest.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(rangedPower);
        }
    }

    // ── 魔法攻撃（範囲攻撃） ──
    void MagicAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, magicRange, enemyLayer);
        if (hitEnemies.Length == 0) return;

        if (magicEffectPrefab != null)
        {
            Instantiate(magicEffectPrefab, transform.position, Quaternion.identity);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("魔法攻撃がヒット: " + enemy.name);
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(magicPower);
            }
        }
    }

    void AutoAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PhysicalAttack();
            lastAttackTime = Time.time;
        }

        if (canRangedAttack && Time.time - lastRangedTime >= rangedCooldown)
        {
            RangedAttack();
            lastRangedTime = Time.time;
        }

        if (canMagicAttack && Time.time - lastMagicTime >= magicCooldown)
        {
            MagicAttack();
            lastMagicTime = Time.time;
        }
    }

    void LevelUp()
    {
        if (exp >= levelUpExp)
        {
            nowLevel++;
            exp -= levelUpExp;
            levelUpExp += 5; // 次のレベルアップに必要な経験値を増やす
            uIController.SetSllimeLevel(nowLevel);
            Debug.Log("レベルアップ！現在のレベル：" + nowLevel);
        }
    }

    void OnDrawGizmos()
    {
        // 近接範囲
        Gizmos.color = Color.red;
        float attackRadius = playerRadius * attackRangeMultiplier;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // 遠距離範囲
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, rangedRange);

        // 魔法範囲
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, magicRange);
    }

    // 敵(トリガー)に触れた瞬間、自動で呼ばれる関数
    void OnTriggerStay2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                if (Time.time - lastDamageTime >= damageCooldown)
                {
                    TakeDamage(4); // ボスからのダメージ量を4に設定
                    lastDamageTime = Time.time; // ダメージを受けた時間を更新
                }
            }
            else if (Time.time - lastDamageTime >= damageCooldown)
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
            skillData = nearSkillOrb.GetSkillOrb();
            storenSkillslot.ReceiveSkills(skillData);
            //nearSkillOrbs.Add(nearSkillOrb);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            nearSkillOrbs.Remove(nearSkillOrb);
        }
    }

    // Player自身がダメージを受ける関数(Enemy.csのTakeDamageと同じ考え方)
    public void TakeDamage(int damage)
    {
        if (invinCible)
        {
            return; // 無敵状態ならダメージを受けない
        }
        hp -= damage;
        uIController.SetLife(hp);

        if (hp <= 0)
        {
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
        skillData = storenSkillslot.GetSelectedSkill();
        if (skillData == null)
        {
            return;
        }

        int duplicateCount = storenSkillslot.CountDuplicates(skillData);
        float multiplier = duplicateCount > 1 ? 1.5f : 1f;

        StartCoroutine(CopyCoroutine(multiplier));
        ApplySkillEffects(skillData, multiplier);
        Debug.Log("コピーの効果を発動しました！（倍率: " + multiplier + "）");
    }

    public void ActivateAvoidance()
    {
        StartCoroutine(AvoidanceCoroutine());
        Debug.Log("回避の効果を発動しました！");
    }

    public IEnumerator AvoidanceCoroutine()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(inputX, inputY);

        isDashing = true; // ダッシュ状態にする

        float elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * DashSpeed * Time.deltaTime;
            yield return null; // 次のフレームまで待つ
        }

        isDashing = false; // ダッシュ状態を解除する
    }

    public IEnumerator CopyCoroutine(float multiplier)
    {
        originalHp = hp;
        originalMoveSpeed = moveSpeed;
        originalSprite = spriteRenderer.sprite;

        hp = skillData.copiedHp;
        moveSpeed = skillData.copiedMoveSpeed;
        spriteRenderer.sprite = skillData.copiedSprite;

        yield return new WaitForSeconds(copyDuration);

        hp = originalHp;
        moveSpeed = originalMoveSpeed;
        spriteRenderer.sprite = originalSprite;

        RemoveSkillEffects(skillData); // Copy終了と同時に効果も解除

        skillData = null;
    }

    public void ApplySkillEffects(SkillData skill, float multiplier)
    {
        foreach (Effect effect in skill.effects)
        {
            effect.ApplyEffect(this, multiplier);
        }
    }

    public void RemoveSkillEffects(SkillData skill)
    {
        foreach (Effect effect in skill.effects)
        {
            effect.RemoveEffect(this);
        }
    }
}