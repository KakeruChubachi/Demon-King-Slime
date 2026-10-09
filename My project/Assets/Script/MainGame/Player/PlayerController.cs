using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    Vector2 facingDirection = Vector2.right;
    public Vector2 FacingDirection => facingDirection; // ★追加：他のスクリプトから向きを読む用
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
    Enemy nearEnemy;

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

    public GameObject bulletPrefab;       // プレイヤーが撃つ弾のプレハブ
    public Transform bulletSpawnPoint;

    [Header("棍棒攻撃")]
    public GameObject konbouVisual;
    public float konbouSwingTime = 0.2f;

    //元のステータスを保存する変数(Spaceコピーの変身用)
    int originalHp;
    float originalMoveSpeed;
    Sprite originalSprite;

    public bool canRangedAttack = false; // 遠距離攻撃が使えるかどうか
    public bool canMagicAttack = false; // 魔法攻撃が使えるかどうか

    public ParticleSystem levelUpParticle;
    public float stretchAmount = 1.3f;
    public float stretchDuration = 0.4f;
    public float stretchHoldTime = 0.2f;

    void Start()
    {
        uIController.SetSllimeLevel(nowLevel);
        uIController.SetLife(hp);
        barrierVisual.SetActive(false); // バリア状態のビジュアルを非表示にする
        uIController.SetExp(exp, levelUpExp);
    }

    // Update is called once per frame
    void Update()
    {
        //入力を調べる
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        //移動ベクトルを作る
        Vector2 moveDirection = new Vector2(inputX, inputY);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            facingDirection = moveDirection.normalized;
        }

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
        if (nearEnemy != null && nearEnemy.CanBeCopied && Input.GetKeyDown(KeyCode.Space))
        {
            CopyFromEnemy(nearEnemy);
        }
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
                e.TakeDamage(physicalPower, Enemy.AttackType.Physical);
            }
        }
    }

    // ── 棍棒スキル ──
    public void KonbouAttack(int damage)
    {

        StartCoroutine(KonbouSwing());//こん棒ふる

        Debug.Log("★★★ 棍棒攻撃を発動しました！ ダメージ：" + damage);

        Vector2 attackPosition =
            (Vector2)transform.position + facingDirection * 1.5f;

        Collider2D[] hitEnemies =
            Physics2D.OverlapCircleAll(attackPosition, 1.0f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            if (e != null)
            {
                Debug.Log("棍棒攻撃がヒット: " + enemy.name);

                e.TakeDamage(damage, Enemy.AttackType.Physical);
            }
        }
    }

    IEnumerator KonbouSwing()
    {
        if (konbouVisual == null) yield break;

        konbouVisual.SetActive(true);

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        konbouVisual.transform.localPosition = facingDirection * 2.0f;

        for (float t = 0; t < 1; t += Time.deltaTime / konbouSwingTime)
        {
            float swingAngle = Mathf.Lerp(angle + 60f, angle - 60f, t);
            konbouVisual.transform.rotation = Quaternion.Euler(0, 0, swingAngle);
            yield return null;
        }

        konbouVisual.SetActive(false);
    }

    public void HeroSwordAttack(int damage)
    {
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(transform.position, 3f, enemyLayer);

        foreach (Collider2D hit in enemies)
        {
            Vector2 direction =
                ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;

            if (Vector2.Angle(facingDirection, direction) <= 45f)
            {

                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy != null)
                {
                    Debug.Log("勇者の剣がヒット：" + enemy.name);
                    enemy.TakeDamage(damage, Enemy.AttackType.Physical);
                }
            }
        }
    }

    // 範囲攻撃スキル
    public void WideAttack(int damage)
    {
        Vector2 attackPosition = (Vector2)transform.position + facingDirection * 1.0f;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, 2.5f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            if (e != null)
            {
                e.TakeDamage(damage, Enemy.AttackType.Physical);
            }
        }
    }

    // ── 遠距離攻撃 ──
    void RangedAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, rangedRange, enemyLayer);
        if (hitEnemies.Length == 0) return;
        Debug.Log("★★★ 見つかった敵の数: " + hitEnemies.Length);

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


        // 一番近い敵への方向を計算
        Vector2 direction = ((Vector2)nearest.transform.position - (Vector2)transform.position).normalized;


        // 弾を発射
        if (bulletPrefab != null)
        {
            Vector3 spawnPosition = transform.position;

            if (bulletSpawnPoint != null)
            {
                spawnPosition = bulletSpawnPoint.position;
            }

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            Debug.Log("弾を生成しました:");

            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
        }

        if (rangedEffectPrefab != null)
        {
            Instantiate(rangedEffectPrefab, nearest.transform.position, Quaternion.identity);
        }

        Enemy e = nearest.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(rangedPower, Enemy.AttackType.Ranged);
        }
    }


    // ── 遠距離攻撃（スキル） ──
    public void BulletSkill(int damage)
    {

        RangedAttack();
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
                e.TakeDamage(magicPower, Enemy.AttackType.Magic);
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
            levelUpExp += 5;

            hp += 3;
            physicalPower += 2;
            rangedPower += 2;
            magicPower += 2;

            uIController.SetExp(exp, levelUpExp);
            uIController.SetSllimeLevel(nowLevel);
            uIController.SetLife(hp);

            StartCoroutine(LevelUpEffectCoroutine());

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

        // 棍棒攻撃の範囲
        Gizmos.color = Color.yellow;

        Vector2 konbouPosition =
            (Vector2)transform.position + facingDirection * 2.0f;

        Gizmos.DrawWireSphere(konbouPosition, 1.0f);

        // 勇者の剣の攻撃範囲
        Gizmos.color = Color.green;

        Vector3 left = Quaternion.Euler(0, 0, 45) * facingDirection * 3f;
        Vector3 right = Quaternion.Euler(0, 0, -45) * facingDirection * 3f;

        Gizmos.DrawLine(transform.position, transform.position + left);
        Gizmos.DrawLine(transform.position, transform.position + right);
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
            nearEnemy = enemy;
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
                TakeDamage(enemy.attackPower); // 仮のダメージ量
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
            uIController.SetExp(exp, levelUpExp);
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



        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        SkillOrb nearSkillOrb = other.GetComponent<SkillOrb>();
        if (nearSkillOrb != null)
        {
            nearSkillOrbs.Remove(nearSkillOrb);
        }

        Enemy exitEnemy = other.GetComponent<Enemy>();
        if (exitEnemy != null && exitEnemy == nearEnemy)
        {
            nearEnemy = null;
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

            // ★現在持っているスキルをリザルト用に保存
            storenSkillslot.SaveSkillsForResult();

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

    // スロット発動: 見た目・ステータスは変えず、効果(攻撃方法など)だけ付与する
    public void ActivateCopy()
    {
        skillData = storenSkillslot.GetSelectedSkill();
        if (skillData == null)
        {
            return;
        }

        int duplicateCount = storenSkillslot.CountDuplicates(skillData); // 倍率は消す前に計算
        float multiplier = duplicateCount > 1 ? 1.5f : 1f;

        StartCoroutine(EffectOnlyCoroutine(skillData));
        ApplySkillEffects(skillData, multiplier);
        storenSkillslot.ConsumeSelectedSkill(); // 使ったスロットを空にする
        Debug.Log("コピーの効果を発動しました！（倍率: " + multiplier + "）");
    }

    // Spaceコピー: 見た目・HP・移動速度も敵のものに変わる
    public void CopyFromEnemy(Enemy target)
    {
        skillData = target.GetCopySkillData();
        target.ConsumeForCopy();
        nearEnemy = null;

        StartCoroutine(CopyCoroutine(1f));
        ApplySkillEffects(skillData, 1f);
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

    // Spaceコピー用: 変身あり
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

    // スロット発動用: 効果だけ付与して、時間が来たら解除
    public IEnumerator EffectOnlyCoroutine(SkillData skill)
    {
        yield return new WaitForSeconds(copyDuration);
        RemoveSkillEffects(skill);
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

    public IEnumerator LevelUpEffectCoroutine()
    {
        if (levelUpParticle != null)
        {
            levelUpParticle.Play();
        }

        Vector3 originalScale = transform.localScale;
        Vector3 stretchedScale = new Vector3(originalScale.x, originalScale.y * stretchAmount, originalScale.z);

        float elapsed = 0f;
        while (elapsed < stretchDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, stretchedScale, elapsed / stretchDuration);
            yield return null;
        }
        transform.localScale = stretchedScale;

        yield return new WaitForSeconds(stretchHoldTime); // 伸びた状態を少しキープ

        elapsed = 0f;
        while (elapsed < stretchDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(stretchedScale, originalScale, elapsed / stretchDuration);
            yield return null;
        }
        transform.localScale = originalScale;
    }
}