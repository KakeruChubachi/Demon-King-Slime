using UnityEngine;

public class MagicAttack : Effect
{
    public int magicAttackAmount = 5;
    public override void ApplyEffect(Player player)
    {
        //player.magicAttack += magicAttackAmount;
        Debug.Log("–‚–@UŒ‚—Í‚ª" + magicAttackAmount + "‘‰Á‚µ‚Ü‚µ‚½B");
    }
}
