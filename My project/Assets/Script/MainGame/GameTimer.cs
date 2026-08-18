using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public UIController uIController;
    public float timeLimit = 180f; //制限時間を設定（秒）
    public float BossApprearamceTime = 1.5f; //ボス出現時間を設定（秒）
    public bool isBossAppeared = false; //ボスが出現したかどうかのフラグ
    public bool isTimeUp = false; //時間切れかどうかのフラグ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit -= Time.deltaTime;
        if (timeLimit < 0)
        {
            timeLimit = 0;
            isTimeUp = true;
        }
        uIController.SetTime(timeLimit);

        if(timeLimit <= 0)
        {
            BossApprearamceTime -= Time.deltaTime;
            if(BossApprearamceTime < 0)
            {
                BossApprearamceTime = 0;
            }
            if(BossApprearamceTime <= 0)
            {
                if (!isBossAppeared)
                {
                    isBossAppeared = true;
                    //ボス出現処理
                    Debug.Log("ボス出現");
                }
            }
        }
    }
}
