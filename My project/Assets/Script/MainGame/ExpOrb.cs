using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    float drawnspeed = 2.0f;//オーブが吸い寄せられる速度
    public Transform target;
    int expAmount = 10;//経験値量

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        Vector3 direction = target.position - transform.position;
        direction = direction.normalized;
        Vector3 movement = direction * drawnspeed * Time.deltaTime;
        transform.position += movement;
    }

    public int PickupExp()
    {
        Destroy(gameObject);
        return expAmount;
    }
}
