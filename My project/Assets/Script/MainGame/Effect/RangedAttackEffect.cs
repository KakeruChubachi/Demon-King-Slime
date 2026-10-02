using UnityEngine;

[CreateAssetMenu(menuName = "Effects/RangedAttack")]
public class RangedAttackEffect : Effect
{
    public override void ApplyEffect(Player player, float multiplier)
    {
        player.canRangedAttack = true;
    }

    public override void RemoveEffect(Player player)
    {
        player.canRangedAttack = false;
    }
}