using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;         // FadeImageをInspectorで設定
    public float fadeDuration = 1f; // フェードにかける時間(秒)

    void Start()
    {
        // TODO: Fade(開始の透明度, 終わりの透明度) をコルーチンとして開始する
        // ヒント: これまで使ってきた StartCoroutine(...) の書き方と同じ形
        StartCoroutine(Fade(1f, 0f));
    }

    void Awake()
    {
        // ① シーンをまたいでも消えないようにする
        // TODO: DontDestroyOnLoad を使う(対象は gameObject)
        DontDestroyOnLoad(gameObject);
    }

    // ② 外部から呼び出す入口。「このシーン名に遷移して」とお願いされる
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    // ③ フェードアウト→シーン切り替え→フェードインを行うコルーチン
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        // (a) フェードアウト(透明→不透明)
        yield return StartCoroutine(Fade(0f, 1f));

        // (b) シーンを切り替える
        // TODO: SceneManager.LoadScene(sceneName) を呼ぶ
        SceneManager.LoadScene(sceneName);

        // (c) フェードイン(不透明→透明)
        yield return StartCoroutine(Fade(1f, 0f));
    }

    // ④ 透明度をstartAlphaからendAlphaまで、fadeDuration秒かけて変化させる
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // TODO: 経過時間の割合(0〜1)を計算する
            float t = elapsedTime / fadeDuration;// ヒント: elapsedTime を fadeDuration で割る

            // TODO: startAlpha から endAlpha へ、tの割合で補間した値を計算する
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);// ヒント: Mathf.Lerp(開始値, 終了値, 割合) という命令がある

            // TODO: fadeImage の色(color)のアルファ値(透明度)だけを alpha に差し替える
            // ヒント: 一度 fadeImage.color を変数に受け取り、その .a だけ書き換えてから、また fadeImage.color に戻す
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;

            yield return null; // 1フレーム待つ
        }

        // ループを抜けたら、最終的に確実にendAlphaへ合わせておく
        // TODO: fadeImage.color のアルファ値を endAlpha にする(誤差対策)
        Color finalColor = fadeImage.color;
        finalColor.a = endAlpha;
        fadeImage.color = finalColor;
    }
}