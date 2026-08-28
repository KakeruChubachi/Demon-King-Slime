using UnityEngine;

public class Physics : Effect
{
    public int physicsAmount = 5;
    public override void ApplyEffect(Player player)
    {
        //player.physics += physicsAmount;
        Debug.Log("•¨—UŒ‚—Í‚ª" + physicsAmount + "‘‰Á‚µ‚Ü‚µ‚½B");
    }
}
