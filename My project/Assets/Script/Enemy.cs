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

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GameObject Orb = Instantiate(spawnOrb, transform.position, Quaternion.identity);
            ExpOrb expOrb = Orb.GetComponent<ExpOrb>();
            expOrb.target = target;
            if (Random.value < dropRate)
            {// ドロップ率に応じてアイテムをドロップするか判定

                int skills = Random.Range(0, skillDatas.Length);
                SkillData skillData = skillDatas[skills];
                GameObject skillOrb = Instantiate(SkillOrbprefab, transform.position, Quaternion.identity);
                SkillOrb skillOrbComponent = skillOrb.GetComponent<SkillOrb>();
                skillOrbComponent.skillData = skillData;
                skillOrbComponent.target = target;
            }
           
            Destroy(gameObject);
        }
    }

    void Update()
    {
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
