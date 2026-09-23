using UnityEngine;

[CreateAssetMenu(fileName = "PhysicalBoostEffect", menuName = "Skills/PhysicalBoostEffect")]
public class PhysicalBoostEffect : Effect
{
    public int baseAmount = 3;
    int appliedAmount;

    public override void ApplyEffect(Player player, float multiplier)
    {
        appliedAmount = Mathf.RoundToInt(baseAmount * multiplier);
        player.physicalPower += appliedAmount;
    }

    public override void RemoveEffect(Player player)
    {
        player.physicalPower -= appliedAmount;
    }
}