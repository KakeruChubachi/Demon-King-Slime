using UnityEngine;

[CreateAssetMenu(fileName = "Newkonbou", menuName = "Skill/Effect/konbou")]
public class konbou : Effect
{
    public int physicalAttackAmount = 5;
    int appliedAmount;

    public override void ApplyEffect(Player player, float multiplier)
    {

        Debug.Log("šššš konbou‚ÌApplyEffect‚É“ü‚è‚Ü‚µ‚½I");
        appliedAmount = Mathf.RoundToInt(physicalAttackAmount * multiplier);

        player.KonbouAttack(appliedAmount);

        Debug.Log("–_UŒ‚‚ğ”­“®‚µ‚Ü‚µ‚½Bƒ_ƒ[ƒWF" + appliedAmount);
    }

    public override void RemoveEffect(Player player)
    {
        // –_UŒ‚‚Í”­“®‚É1‰ñ‚¾‚¯UŒ‚‚·‚é‚Ì‚Å‰½‚à‚µ‚È‚¢
    }
}