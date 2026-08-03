using UnityEngine;

public class EnemySpawnenPosition : MonoBehaviour
{
    public float outsideDistance = 5f;

    public Vector3 GetSpawnPosition()
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector3 center = cam.transform.position;

        float left = center.x - width;
        float right = center.x + width;
        float top = center.y + height;
        float bottom = center.y - height;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // è„
                return new Vector3(Random.Range(left, right), top + outsideDistance, 0);

            case 1: // â∫
                return new Vector3(Random.Range(left, right), bottom - outsideDistance, 0);

            case 2: // ç∂
                return new Vector3(left - outsideDistance, Random.Range(bottom, top), 0);

            default: // âE
                return new Vector3(right + outsideDistance, Random.Range(bottom, top), 0);
        }
    }
}