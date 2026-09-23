using UnityEngine;

[CreateAssetMenu(fileName = "MagicBoostEffect", menuName = "Skills/MagicBoostEffect")]
public class MagicBoostEffect : Effect
{
    public int baseAmount = 4;
    int appliedAmount;
    bool wasEnabled;

    public override void ApplyEffect(Player player, float multiplier)
    {
        wasEnabled = player.canMagicAttack;
        player.canMagicAttack = true;
        appliedAmount = Mathf.RoundToInt(baseAmount * multiplier);
        player.magicPower += appliedAmount;
    }

    public override void RemoveEffect(Player player)
    {
        player.magicPower -= appliedAmount;
        player.canMagicAttack = wasEnabled;
    }
}