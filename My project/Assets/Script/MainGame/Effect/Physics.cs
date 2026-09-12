using UnityEngine;

[CreateAssetMenu(fileName = "NewPhysicsEffect", menuName = "Skill/Effect/Physics")]
public class Physics : Effect
{
    public int physicsAmount = 5;
    int appliedAmount;

    public override void ApplyEffect(Player player, float multiplier)
    {
        appliedAmount = Mathf.RoundToInt(physicsAmount * multiplier);
        player.physicalPower += appliedAmount;
        Debug.Log("•¨—UŒ‚—Í‚ª" + appliedAmount + "‘‰Á‚µ‚Ü‚µ‚½B");
    }

    public override void RemoveEffect(Player player)
    {
        player.physicalPower -= appliedAmount;
        Debug.Log("•¨—UŒ‚—Í‚Ì‘‰Á‚ªI—¹‚µ‚Ü‚µ‚½B");
    }
}