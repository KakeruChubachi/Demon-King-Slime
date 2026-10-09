using UnityEngine;

[CreateAssetMenu(fileName = "NewWideAttack", menuName = "Skill/Effect/WideAttack")]
public class WideAttack : Effect
{
    public int physicalAttackAmount = 5;
    int appliedAmount;

    public override void ApplyEffect(Player player, float multiplier)
    {
        appliedAmount = Mathf.RoundToInt(physicalAttackAmount * multiplier);

        player.WideAttack(appliedAmount);

        Debug.Log("ボス棍棒攻撃を発動しました。ダメージ：" + appliedAmount);
    }

    public override void RemoveEffect(Player player)
    {
    }
}
