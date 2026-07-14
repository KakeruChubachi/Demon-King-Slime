using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float playerRadius = 1.0f;
    public float attackRangeMultiplier = 1.5f;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        /*AutoAttack();*/
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
        }
    }

    /*オート攻撃の仮実装
     * void AutoAttack()
     * {
     *      一定間隔で自動的にAttack()を呼び出す処理を実装する
     * }
     */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float attackRadius = playerRadius * attackRangeMultiplier;

        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
