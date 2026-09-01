using UnityEngine;
using System.Collections;

public class Boss : Enemy
{
    public float tackleSpeed = 8f;//タックルの速度
    public float tackleDistance = 10f;//タックルの距離
    public GameObject dashWarning; // ゲームオブジェクトの参照
    public GameObject dashWarningVisual; // 警告のビジュアルの参照
    float dashWarningMultiplier = 0.2f; // 警告の幅をタックル距離に設定するための倍率

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
            Vector3 direction = target.position - transform.position;
            direction = direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            dashWarning.transform.eulerAngles = new Vector3(0f,0f,angle);
            Vector3 newScale = dashWarningVisual.transform.localScale;
            newScale.x = tackleDistance * dashWarningMultiplier; // 警告の幅をタックル距離に設定
            dashWarningVisual.transform.localScale = newScale;
            dashWarning.SetActive(true); // 警告を表示
            yield return new WaitForSeconds(0.5f); // 0.5秒間停止

            
            float distanceTraveled = 0f; // タックルの移動距離を保存する変数
            while (distanceTraveled < tackleDistance)
            {
                float Distancecovered = tackleSpeed * Time.deltaTime; // タックルの移動距離を計算
                transform.position += direction * Distancecovered; // タックルの移動
                distanceTraveled += Distancecovered; // 移動距離を更新
                yield return null; // 次のフレームまで待機
            }
            dashWarning.SetActive(false); // 警告を非表示
        }
    }

}
