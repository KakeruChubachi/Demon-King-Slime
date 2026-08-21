using UnityEngine;

public class HPboost : Effect
{
    public int HPamount = 5;
    public override void ApplyEffect(Player player)
    {
        player.hp += HPamount;
    }
}
