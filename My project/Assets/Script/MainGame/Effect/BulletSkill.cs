using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletSkill", menuName = "Skill/Effect/BulletSkill")]
public class BulletSkill : Effect
{

    public override void ApplyEffect(Player player, float multiplier)
    {
        player.BulletSkill();

        Debug.Log("‰“‹——£UŒ‚‚ğ”­“®‚µ‚Ü‚µ‚½B");
    }

    public override void RemoveEffect(Player player)
    {

    }
}