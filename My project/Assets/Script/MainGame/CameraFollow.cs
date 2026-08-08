using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // í«è]Ç∑ÇÈëŒè€ÇÃTransform

    void Update()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = newPosition;
    }
}
