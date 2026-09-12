using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public GameObject[] bossPrefabs = new GameObject[3]; // ボスを3体設定（Boss, Boss2, Boss3）
    public UIController uIController;
    public float[] phaseTimeLimits = { 60f, 60f, 60f }; // 各フェーズの制限時間
    public float bossAppearanceDelay = 1.5f; // ボス出現までの予告時間

    public int currentPhase = 0; // 現在のフェーズ（0, 1, 2）
    public bool isBossAppeared = false;
    public bool isTimeUp = false;

    float timeLimit;
    float bossApprearanceTime;
    GameObject currentBossInstance;

    void Start()
    {
        timeLimit = phaseTimeLimits[currentPhase];
        bossApprearanceTime = bossAppearanceDelay;
    }

    void Update()
    {
        // 全フェーズクリア済みなら何もしない
        if (currentPhase >= bossPrefabs.Length) return;

        if (!isBossAppeared)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0)
            {
                timeLimit = 0;
                isTimeUp = true;
            }
            uIController.SetTime(timeLimit);

            if (isTimeUp)
            {
                bossApprearanceTime -= Time.deltaTime;
                if (bossApprearanceTime <= 0)
                {
                    isBossAppeared = true;
                    Debug.Log((currentPhase + 1) + "体目のボス出現");
                    currentBossInstance = Instantiate(bossPrefabs[currentPhase], new Vector3(0, 5, 0), Quaternion.identity);
                }
            }
        }
        else
        {
            // ボスが倒されて破棄されたら次のフェーズへ
            if (currentBossInstance == null)
            {
                currentPhase++;
                Debug.Log((currentPhase) + "フェーズ目クリア");

                if (currentPhase < bossPrefabs.Length)
                {
                    // 次のフェーズの準備
                    isBossAppeared = false;
                    isTimeUp = false;
                    timeLimit = phaseTimeLimits[currentPhase];
                    bossApprearanceTime = bossAppearanceDelay;
                }
                else
                {
                    Debug.Log("全ボスを撃破しました！ゲームクリア");
                    // TODO: クリア画面への遷移などをここに追加
                }
            }
        }
    }
}