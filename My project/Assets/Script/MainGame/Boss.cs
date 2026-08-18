using UnityEngine;
using System.Collections;

public class Boss : Enemy
{
    public float tackleSpeed = 8f;//タックルの速度
    public float tackleDistance = 10f;//タックルの距離
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossMovesCoroutine());
    }

    public IEnumerator BossMovesCoroutine()
    {
        
        while (true)
        {
            movementEnabled = true; // 移動を有効にする
            yield return new WaitForSeconds(1f); // 1秒間移動
            movementEnabled = false; // 移動を無効にする
            yield return new WaitForSeconds(0.5f); // 0.5秒間停止

            Vector3 direction = target.position - transform.position;
            direction = direction.normalized;
            float distanceTraveled = 0f; // タックルの移動距離を保存する変数
            while (distanceTraveled < tackleDistance)
            {
                float Distancecovered = tackleSpeed * Time.deltaTime; // タックルの移動距離を計算
                transform.position += direction * Distancecovered; // タックルの移動
                distanceTraveled += Distancecovered; // 移動距離を更新
                yield return null; // 次のフレームまで待機
            }
            
        }
    }

}
