using UnityEngine;

[CreateAssetMenu(fileName = "NewLongdistanceEffect", menuName = "Skill/Effect/Longdistance")]
public class Longdistance : Effect
{
    public int longdistanceAmount = 5;
    int appliedAmount;

    public override void ApplyEffect(Player player, float multiplier)
    {
        appliedAmount = Mathf.RoundToInt(longdistanceAmount * multiplier);
        player.rangedPower += appliedAmount;
        player.canRangedAttack = true; // ’Ç‰ÁF‰“‹——£UŒ‚‚ğ‰ğ‹Ö
        Debug.Log("‰“‹——£UŒ‚—Í‚ª" + appliedAmount + "‘‰Á‚µ‚Ü‚µ‚½B");
    }

    public override void RemoveEffect(Player player)
    {
        player.rangedPower -= appliedAmount;
        player.canRangedAttack = false; // ’Ç‰ÁFŒø‰ÊI—¹‚Æ“¯‚É‹Ö~‚É–ß‚·
        Debug.Log("‰“‹——£UŒ‚—Í‚Ì‘‰Á‚ªI—¹‚µ‚Ü‚µ‚½B");
    }
}