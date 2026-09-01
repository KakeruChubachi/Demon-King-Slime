using UnityEngine;

public class Debuff : Effect
{
    public int debuffAmount = 5;
    public int debuffType = 0;
    public int debuffRate = 0;
    public int debuffRateType = 0;

    public override void ApplyEffect(Player player)
    {
        //player.debuff += debuffAmount;
        Debug.Log("デバフが" + debuffAmount + "増加しました。");
    }
}
