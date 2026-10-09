using UnityEngine;

[CreateAssetMenu(fileName = "FanAttack", menuName = "Effects/Fan Attack")]
public class FanAttackEffect : Effect
{
    public GameObject swingPrefab;   // Œ©‚½–Ú—pi”CˆÓj
    public float baseDamage = 15f;
    public float interval = 1.2f;
    public float range = 3f;
    [Range(10f, 360f)] public float angle = 90f;

    public override void ApplyEffect(Player player, float multiplier)
    {
        var fan = player.GetComponent<FanAttacker>();
        if (fan == null) fan = player.gameObject.AddComponent<FanAttacker>();
        fan.Setup(swingPrefab, baseDamage * multiplier, interval, range, angle);
    }

    public override void RemoveEffect(Player player)
    {
        var fan = player.GetComponent<FanAttacker>();
        if (fan != null) Destroy(fan);
    }
}