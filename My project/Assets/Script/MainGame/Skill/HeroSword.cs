using UnityEngine;

[CreateAssetMenu(fileName = "HeroSword", menuName = "Skill/Effect/HeroSword")]
public class HeroSword : Effect
{
    public int damage = 5;

    public override void ApplyEffect(Player player, float multiplier)
    {
        player.HeroSwordAttack(Mathf.RoundToInt(damage * multiplier));
    }

    public override void RemoveEffect(Player player)
    {
    }
}