using UnityEngine;

[CreateAssetMenu(fileName = "RangedFireAttack", menuName = "Effects/Ranged Fire Attack")]
public class RangedFireAttackEffect : Effect
{
    public FireProjectile projectilePrefab;
    public float baseDamage = 10f;
    public float interval = 1.5f;
    public float range = 8f;

    public override void ApplyEffect(Player player, float multiplier)
    {
        var shooter = player.GetComponent<FireShooter>();
        if (shooter == null) shooter = player.gameObject.AddComponent<FireShooter>();
        shooter.Setup(projectilePrefab, baseDamage * multiplier, interval, range);
    }

    public override void RemoveEffect(Player player)
    {
        var shooter = player.GetComponent<FireShooter>();
        if (shooter != null) Destroy(shooter);
    }
}