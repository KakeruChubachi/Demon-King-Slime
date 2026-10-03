using System.Collections;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    [Header("タイトルロゴ")]
    [SerializeField] private RectTransform titleLogo;

    [Header("最初の黒い画面")]
    [SerializeField] private CanvasGroup fadeImage;

    [Header("順番に表示するボタン")]
    [SerializeField] private CanvasGroup[] menuButtons;

    [Header("ロゴが落ちる距離")]
    [SerializeField] private float startHeight = 500f;

    [Header("ロゴが落ちる時間")]
    [SerializeField] private float logoMoveTime = 1f;

    [Header("黒い画面が消える時間")]
    [SerializeField] private float fadeTime = 0.5f;

    [Header("各ボタンの表示時間")]
    [SerializeField] private float buttonFadeTime = 0.4f;

    [Header("ボタン同士の表示間隔")]
    [SerializeField] private float buttonInterval = 0.2f;

    [Header("動き始めるまでの時間")]
    [SerializeField] private float startWaitTime = 0.5f;

    [Header("ロゴ表示後の待ち時間")]
    [SerializeField] private float logoWaitTime = 0.5f;

    private Vector2 logoStartPosition;
    private Vector2 logoEndPosition;

    private void Start()
    {
        logoEndPosition = titleLogo.anchoredPosition;

        logoStartPosition =
            logoEndPosition + new Vector2(0f, startHeight);

        titleLogo.anchoredPosition = logoStartPosition;

        fadeImage.alpha = 1f;
        fadeImage.gameObject.SetActive(true);
        fadeImage.blocksRaycasts = true;

        foreach (CanvasGroup button in menuButtons)
        {
            if (button == null)
            {
                continue;
            }

            button.alpha = 0f;
            button.interactable = false;
            button.blocksRaycasts = false;
        }

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(startWaitTime);

        float timer = 0f;

        while (timer < logoMoveTime)
        {
            timer += Time.deltaTime;

            float rate = timer / logoMoveTime;

            rate = 1f - Mathf.Pow(1f - rate, 3f);

            titleLogo.anchoredPosition =
                Vector2.Lerp(
                    logoStartPosition,
                    logoEndPosition,
                    rate
                );

            yield return null;
        }

        titleLogo.anchoredPosition = logoEndPosition;

        yield return new WaitForSeconds(logoWaitTime);

        yield return StartCoroutine(
            FadeCanvasGroup(fadeImage, 1f, 0f, fadeTime)
        );

        fadeImage.blocksRaycasts = false;

        foreach (CanvasGroup button in menuButtons)
        {
            if (button == null)
            {
                continue;
            }

            yield return StartCoroutine(
                FadeCanvasGroup(
                    button,
                    0f,
                    1f,
                    buttonFadeTime
                )
            );

            button.interactable = true;
            button.blocksRaycasts = true;

            yield return new WaitForSeconds(buttonInterval);
        }
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup target,
        float startAlpha,
        float endAlpha,
        float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float rate = timer / duration;

            target.alpha =
                Mathf.Lerp(startAlpha, endAlpha, rate);

            yield return null;
        }

        target.alpha = endAlpha;
    }
}