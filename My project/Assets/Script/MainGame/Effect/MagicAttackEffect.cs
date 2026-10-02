using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MagicAttack")]
public class MagicAttackEffect : Effect
{
    public override void ApplyEffect(Player player, float multiplier)
    {
        player.canMagicAttack = true;
    }

    public override void RemoveEffect(Player player)
    {
        player.canMagicAttack = false;
    }
}