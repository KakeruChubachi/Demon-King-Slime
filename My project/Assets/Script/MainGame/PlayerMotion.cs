using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    public float widthAmount = 0.15f;
    public float heightAmount = 0.20f;
    public float speed = 10f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float wave = Mathf.Sin(Time.time * speed);

        // ècÇ…êLÇ—ÇÈ
        float yScale = 1f + wave * heightAmount;

        // â°Ç…Ç¬Ç‘ÇÍÇÈ
        float xScale = 1f - wave * widthAmount;

        transform.localScale = new Vector3(
            originalScale.x * xScale,
            originalScale.y * yScale,
            originalScale.z);
    }
}
