using UnityEngine;
using System.Collections;

public class TitleSlimeDrop : MonoBehaviour
{
    public float fallSpeed = 500f;
    public float groundY = 0f;

    private RectTransform rect;
    private bool landed = false;

    // Å‰‚Ì‰æ‘œ‚Ì‘å‚«‚³‚ğ•Û‘¶
    private Vector3 originalScale;

    void Start()
    {
        rect = GetComponent<RectTransform>();

        // Å‰‚Ì‘å‚«‚³‚ğ•Û‘¶
        originalScale = rect.localScale;
    }

    void Update()
    {
        if (!landed)
        {
            // ‰º‚É—‚Æ‚·
            rect.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;

            // ’n–Ê‚É’…‚¢‚½‚ç
            if (rect.anchoredPosition.y <= groundY)
            {
                rect.anchoredPosition = new Vector2(
                    rect.anchoredPosition.x,
                    groundY
                );

                landed = true;

                StartCoroutine(Poyon());
            }
        }
    }

    IEnumerator Poyon()
    {
        // ’…’n‚µ‚Ä‰¡‚ÉL‚ª‚é
        yield return ScaleTo(
            new Vector3(
                originalScale.x * 1.25f,
                originalScale.y * 0.75f,
                originalScale.z
            ),
            0.25f
        );

        // ‚ä‚Á‚­‚èc‚ÉL‚Ñ‚é
        yield return ScaleTo(
            new Vector3(
                originalScale.x * 0.85f,
                originalScale.y * 1.15f,
                originalScale.z
            ),
            0.5f
        );

        // ­‚µ—h‚ê‚é
        yield return ScaleTo(
            new Vector3(
                originalScale.x * 1.05f,
                originalScale.y * 0.95f,
                originalScale.z
            ),
            0.2f
        );

        // Œ³‚Ì‘å‚«‚³‚É–ß‚·
        yield return ScaleTo(
            originalScale,
            0.25f
        );
    }

    IEnumerator ScaleTo(Vector3 target, float duration)
    {
        Vector3 start = rect.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            rect.localScale = Vector3.Lerp(
                start,
                target,
                t
            );

            yield return null;
        }

        rect.localScale = target;
    }
}
