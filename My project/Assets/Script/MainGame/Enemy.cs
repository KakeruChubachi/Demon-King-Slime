using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 10;
    public float moveSpeed = 2f;   // Enemyの移動速度
    public Transform target;       // 追いかける対象(Playerをinspectorで設定)
    public GameObject spawnOrb;
    public float dropRate = 0.05f; // ドロップ率(0.0～1.0)
    public GameObject SkillOrbprefab;
    public SkillData[] skillDatas; // スキルデータの配列
    public bool movementEnabled = true; // 移動を有効にするかどうかのフラグ
    public SpriteRenderer spriteRenderer;
    public SkillData copiedskillData; // コピーされたスキルデータを保持する変数
    public int ExpbaseValue = 10; // ドロップする経験値の基本値

    protected virtual void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    public void TakeDamage(int damage)
    {
        int SaveHp = hp;// ダメージを受ける前のHPを保存
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log("死亡処理開始");
            GameObject Orb = Instantiate(spawnOrb, transform.position, Quaternion.identity);
            ExpOrb expOrb = Orb.GetComponent<ExpOrb>();
            expOrb.target = target;
            expOrb.SetExpAmount(ExpbaseValue); // ドロップする経験値の量を設定
            if (Random.value < dropRate)
            {// ドロップ率に応じてアイテムをドロップするか判定
                int skills = Random.Range(0, skillDatas.Length);
                SkillData skillData = skillDatas[skills];
                copiedskillData = Instantiate(skillData);            // スキルデータをコピーして新しいインスタンスを作成
                copiedskillData.copiedMoveSpeed = moveSpeed;         // コピーされたスキルデータにEnemyの移動速度を設定
                copiedskillData.copiedSprite = spriteRenderer.sprite;// コピーされたスキルデータにEnemyのスプライトを設定
                copiedskillData.copiedHp = SaveHp;                   // コピーされたスキルデータにEnemyのHPを設定
                GameObject skillOrb = Instantiate(SkillOrbprefab, transform.position, Quaternion.identity);// スキルオーブを生成
                SkillOrb skillOrbComponent = skillOrb.GetComponent<SkillOrb>();// スキルオーブのコンポーネントを取得
                skillOrbComponent.skillData = copiedskillData;// スキルオーブにコピーされたスキルデータを設定
                skillOrbComponent.target = target;// スキルオーブのターゲットを設定
            }
            Debug.Log("Destroy呼び出し");
            Destroy(gameObject);
        }
    }

    protected virtual void Update()
    {
        if (!movementEnabled)
        {
            return;
        }
        // ① Player方向への差(ベクトル)を求める
        Vector3 direction = target.position - transform.position;// TODO: target.position - transform.position

        // ② 方向だけを取り出す(長さを1にする)
        direction = direction.normalized;// TODO: direction を正規化する(ヒント: .normalized というプロパティがある)

        // ③ 移動速度と時間を掛けて、実際の移動量にする
        Vector3 movement = direction * moveSpeed * Time.deltaTime;// TODO: direction * moveSpeed * Time.deltaTime

        // ④ 位置を更新する
        // TODO: transform.position に movement を加算する
        transform.position += movement;
    }
}
