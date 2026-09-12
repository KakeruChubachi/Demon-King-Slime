using UnityEngine;

[CreateAssetMenu(fileName = "NewMagicAttackEffect", menuName = "Skill/Effect/MagicAttack")]
public class MagicAttack : Effect
{
    public int magicAttackAmount = 5;
    int appliedAmount;

    public override void ApplyEffect(Player player, float multiplier)
    {
        appliedAmount = Mathf.RoundToInt(magicAttackAmount * multiplier);
        player.magicPower += appliedAmount;
        player.canMagicAttack = true; // 追加：魔法攻撃を解禁
        Debug.Log("魔法攻撃力が" + appliedAmount + "増加しました。");
    }

    public override void RemoveEffect(Player player)
    {
        player.magicPower -= appliedAmount;
        player.canMagicAttack = false; // 追加：効果終了と同時に禁止に戻す
        Debug.Log("魔法攻撃力の増加が終了しました。");
    }
}