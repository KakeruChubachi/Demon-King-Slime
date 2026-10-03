using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameTransition : MonoBehaviour
{
    [Header("黒い遷移画面")]
    [SerializeField] private GameObject transitionPanel;

    [Header("小さくなる青い画像")]
    [SerializeField] private RectTransform slimeImage;

    [Header("メインゲームのシーン名")]
    [SerializeField] private string mainGameSceneName = "MainGame";

    [Header("青い画像の最初の大きさ")]
    [SerializeField] private float startScale = 5f;

    [Header("小さくなる時間")]
    [SerializeField] private float shrinkTime = 1f;

    [Header("真っ黒になった後の待ち時間")]
    [SerializeField] private float blackWaitTime = 0.3f;

    private bool isTransitioning;

    private void Start()
    {
        transitionPanel.SetActive(false);
    }

    // スタートボタンから呼び出す
    public void GoToMainGame()
    {
        // スタートボタンの連打を防ぐ
        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        // 黒いパネルを表示
        transitionPanel.SetActive(true);

        // 青い画像を中央に置く
        slimeImage.anchoredPosition = Vector2.zero;

        // 最初は画面全体を覆うくらい大きくする
        slimeImage.localScale =
            new Vector3(startScale, startScale, 1f);

        float timer = 0f;

        // 青い画像を少しずつ小さくする
        while (timer < shrinkTime)
        {
            timer += Time.deltaTime;

            float rate = timer / shrinkTime;

            // 動きをなめらかにする
            rate = rate * rate;

            float scale =
                Mathf.Lerp(startScale, 0f, rate);

            slimeImage.localScale =
                new Vector3(scale, scale, 1f);

            yield return null;
        }

        // 完全に小さくする
        slimeImage.localScale = Vector3.zero;

        // 真っ黒な画面を少し表示
        yield return new WaitForSeconds(blackWaitTime);

        // メインゲームへ移動
        SceneManager.LoadScene(mainGameSceneName);
    }
}