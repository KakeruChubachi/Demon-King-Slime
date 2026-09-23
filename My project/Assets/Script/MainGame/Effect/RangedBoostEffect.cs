using UnityEngine;

[CreateAssetMenu(fileName = "RangedBoostEffect", menuName = "Skills/RangedBoostEffect")]
public class RangedBoostEffect : Effect
{
    public int baseAmount = 2;
    int appliedAmount;
    bool wasEnabled;

    public override void ApplyEffect(Player player, float multiplier)
    {
        wasEnabled = player.canRangedAttack;
        player.canRangedAttack = true;
        appliedAmount = Mathf.RoundToInt(baseAmount * multiplier);
        player.rangedPower += appliedAmount;
    }

    public override void RemoveEffect(Player player)
    {
        player.rangedPower -= appliedAmount;
        player.canRangedAttack = wasEnabled;
    }
}